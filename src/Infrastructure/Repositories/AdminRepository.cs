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
}