using Core.Entities;
using Core.Interfaces;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CollabParticipantRepository : ICollabParticipantRepository
{
    
    private readonly LivePlaygroundDbContext _context;

    public CollabParticipantRepository(LivePlaygroundDbContext context)
    {
        _context = context;
    }


    public async Task<CollabSession?> GetByIdAsync(Guid id)
    {
        
        return await _context.CollabSessions.
            Include(s => s.Owner).
            FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<CollabSession>> GetSessionsWhereUserIsParticipantAsync(Guid userId)
    {
        var sessionIds = await _context.CollabParticipants
            .Where(p => p.UserId == userId)
            .Select(p => p.SessionId)
            .ToListAsync();

        if (!sessionIds.Any())
            return new List<CollabSession>();

        return await _context.CollabSessions
            .Where(s => sessionIds.Contains(s.Id))
            .Include(s => s.CodeSnippet)
            .Include(s => s.Participants)
            .Include(s => s.EditHistories)
            .ThenInclude(h => h.EditedByUser)
            .ToListAsync();
    }




    public async Task<List<CollabSession>> GetSessionsCreatedByUserAsync(Guid userId)
    {
        return await _context.CollabSessions
            .Where(s => s.OwnerId == userId)
            .Include(s => s.CodeSnippet)
            .Include(s => s.Participants)
            .ThenInclude(p => p.User) 
            .Include(s => s.EditHistories)
            .ThenInclude(h => h.EditedByUser) 
            .Include(s => s.Owner) 
            .ToListAsync();
    }

    public async Task<CollabSession?> GetSessionWithParticipantsAsync(Guid sessionId)
    {
        return await _context.CollabSessions
            .Include(s => s.Participants)
            .ThenInclude(p => p.User)
            .Include(s => s.CodeSnippet)
            .FirstOrDefaultAsync(s => s.Id == sessionId);
    }

    public async Task<CollabSession> CreateSessionAsync(CollabSession session)
    {
        
        _context.CollabSessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
        
    }

    public async Task<CollabSession> UpdateSessionAsync(CollabSession session, SessionEditHistory history)
    {

        _context.CollabSessions.Update(session);
        _context.SessionEditHistories.Add(history);
        await _context.SaveChangesAsync();
        return session;

    }
}