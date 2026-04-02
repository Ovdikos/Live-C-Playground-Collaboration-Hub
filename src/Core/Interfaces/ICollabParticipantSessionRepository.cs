using Core.Entities;

namespace Core.Interfaces;

public interface ICollabParticipantSessionRepository
{
    Task<CollabSession?> GetByIdAsync(Guid id);
    Task<List<CollabSession>> GetSessionsWhereUserIsParticipantAsync(Guid userId);
    Task<List<CollabSession>> GetSessionsCreatedByUserAsync(Guid userId);
    
    Task<CollabSession?> GetSessionWithParticipantsAsync(Guid sessionId);
    
    Task<List<SessionEditHistory>> GetCollabSessionHistoryAsync(Guid sessionId);
    
    Task<CollabSession> CreateSessionAsync(CollabSession session);
    
    Task<CollabSession> UpdateSessionAsync(CollabSession session, SessionEditHistory history);
    
    Task AddParticipantByJoinCodeAsync(string joinCode, Guid userId);
    Task RemoveParticipantAsync(Guid sessionId, Guid userId);
    Task DeleteSessionIfOwnerAsync(Guid sessionId, Guid userId);

    
}