using Core.Entities;
using Core.Interfaces;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AdminRepository : IAdminRepository
{
    
    private readonly LivePlaygroundDbContext _db;

    public AdminRepository(LivePlaygroundDbContext db)
    {
        _db = db;
    }


    public async Task<List<User>> GetUsersAsync(string? search = null, DateTime? createdFrom = null, DateTime? createdTo = null,
        bool? isAdmin = null, string? orderBy = null, bool desc = false)
    {
        var query = _db.Users.AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(u => u.Username.Contains(search) || u.Email.Contains(search));
        if (createdFrom.HasValue)
            query = query.Where(u => u.CreatedAt >= createdFrom.Value);
        if (createdTo.HasValue)
            query = query.Where(u => u.CreatedAt <= createdTo.Value);
        if (isAdmin.HasValue)
            query = query.Where(u => u.IsAdmin == isAdmin.Value);

        if (!string.IsNullOrEmpty(orderBy))
        {
            query = orderBy.ToLower() switch
            {
                "username" => desc ? query.OrderByDescending(u => u.Username) : query.OrderBy(u => u.Username),
                "email" => desc ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                "createdat" => desc ? query.OrderByDescending(u => u.CreatedAt) : query.OrderBy(u => u.CreatedAt),
                "id" => desc ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id),
                "isadmin" => desc ? query.OrderByDescending(u => u.IsAdmin) : query.OrderBy(u => u.IsAdmin),
                _ => query.OrderBy(u => u.Username)
            };
        }
        else
        {
            query = query.OrderBy(u => u.Username);
        }

        return await query.ToListAsync();
    }

    public async Task<User?> GetUserDetailsByUsernameAsync(string username)
    {
        return await _db.Users
            .Include(u => u.CodeSnippets)
            .Include(u => u.OwnedSessions)
            .ThenInclude(s => s.CodeSnippet)
            .Include(u => u.CollabParticipants)
            .ThenInclude(cp => cp.Session)
            .ThenInclude(s => s.CodeSnippet)
            .Include(u => u.CollabParticipants)
            .ThenInclude(cp => cp.Session)
            .ThenInclude(s => s.Owner) 
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> SetUserBlockedAsync(Guid userId, bool blocked, string? blockedByEmail)
    {
        
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) throw new KeyNotFoundException();

        user.IsBlocked = blocked;
        user.BlockedByAdminEmail = blocked ? blockedByEmail : null;
        await _db.SaveChangesAsync();
        return true;
        
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user == null)
            return false;
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
        
    }

    public async Task<List<CodeSnippet>> GetAllSnippetsAsync(bool? isPublic = null)
    {
        var query = _db.CodeSnippets.AsQueryable();

        if (isPublic.HasValue)
            query = query.Where(s => s.IsPublic == isPublic.Value);

        return await query
            .Include(s => s.Owner)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<CollabSession>> GetAllSessionsAsync(string? search = null, bool? isActive = null, DateTime? createdFrom = null,
        DateTime? createdTo = null, int? minParticipants = null)
    {
        var query = _db.CollabSessions
            .Include(s => s.Owner)
            .Include(s => s.CodeSnippet)
            .Include(s => s.Participants).ThenInclude(p => p.User)
            .Include(s => s.EditHistories)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(s => s.Name.Contains(search));
        
        if (isActive.HasValue)
            query = query.Where(s => s.IsActive == isActive.Value);

        if (createdFrom.HasValue)
            query = query.Where(s => s.CreatedAt >= createdFrom.Value);

        if (createdTo.HasValue)
            query = query.Where(s => s.CreatedAt <= createdTo.Value);

        if (minParticipants.HasValue)
            query = query.Where(s => s.Participants.Count >= minParticipants);

        query = query.OrderByDescending(s => s.CreatedAt);

        return await query.ToListAsync();
    }

    public async Task<CodeSnippet?> GetSnippetByTitleAsync(string title)
    {
        return await _db.CodeSnippets
            .Include(s => s.Owner)
            .FirstOrDefaultAsync(s => s.Title == title);
    }

    public async Task<bool> UpdateSnippetAsync(CodeSnippet snippet)
    {
        _db.CodeSnippets.Update(snippet);
        var changes = await _db.SaveChangesAsync();
        return changes > 0;
    }

    public async Task<bool> DeleteSnippetAsync(Guid snippetId)
    {
        var snippet = await _db.CodeSnippets.FindAsync(snippetId);
        if (snippet == null)
            return false;

        _db.CodeSnippets.Remove(snippet);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<CollabSession?> GetByIdAsync(Guid id)
    {
        return await _db.CollabSessions
            .Include(s => s.Owner)
            .Include(s => s.Participants).ThenInclude(p => p.User)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<CollabSession?> GetByNameAsync(string name)
    {
        return await _db.CollabSessions
            .Include(s => s.Owner)
            .Include(s => s.CodeSnippet)
            .Include(s => s.Participants)
            .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(s => s.Name == name);
    }

    public async Task<bool> UpdateAsync(CollabSession session)
    {
        _db.CollabSessions.Update(session);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteSessionByNameAsync(string name)
    {
        var session = await _db.CollabSessions
            .FirstOrDefaultAsync(s => s.Name == name);

        if (session == null)
            return false;

        _db.CollabSessions.Remove(session);
        await _db.SaveChangesAsync();
        return true;
    }
}