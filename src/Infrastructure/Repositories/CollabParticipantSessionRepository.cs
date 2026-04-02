using Core.Entities;
using Core.Interfaces;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CollabParticipantSessionRepository : ICollabParticipantSessionRepository
{
    
    private readonly LivePlaygroundDbContext _context;

    public CollabParticipantSessionRepository(LivePlaygroundDbContext context)
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

    public async Task<List<SessionEditHistory>> GetCollabSessionHistoryAsync(Guid sessionId)
    {
        return await _context.SessionEditHistories
                .Where(h => h.SessionId == sessionId)
                .Include(h => h.EditedByUser)
                .OrderByDescending(h => h.EditedAt)
                .ToListAsync();
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

    public async Task AddParticipantByJoinCodeAsync(string joinCode, Guid userId)
    {
        var session = await _context.CollabSessions
            .Include(s => s.Participants)
            .FirstOrDefaultAsync(s => s.JoinCode == joinCode);

        if (session == null) throw new Exception("Session not found!");
        if (session.Participants.Any(p => p.UserId == userId)) throw new Exception("User is already a participant!");

        _context.CollabParticipants.Add(new CollabParticipant
        {
            Id = Guid.NewGuid(),
            SessionId = session.Id,
            UserId = userId,
            JoinedAt = DateTime.UtcNow
        });
    
        await _context.SaveChangesAsync();
    }

    public async Task RemoveParticipantAsync(Guid sessionId, Guid userId)
    {
        var participant = await _context.CollabParticipants
            .FirstOrDefaultAsync(p => p.SessionId == sessionId && p.UserId == userId);

        if (participant == null) throw new Exception("Participant not found in this session.");

        _context.CollabParticipants.Remove(participant);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSessionIfOwnerAsync(Guid sessionId, Guid userId)
    {
        var session = await _context.CollabSessions
            .FirstOrDefaultAsync(s => s.Id == sessionId);

        if (session == null) throw new Exception("Session not found!");
        if (session.OwnerId != userId) throw new UnauthorizedAccessException("Only the owner can delete this session.");

        _context.CollabSessions.Remove(session);
        await _context.SaveChangesAsync();
    }
}