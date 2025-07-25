﻿using Core.Entities;

namespace Core.Interfaces;

public interface ICollabParticipantRepository
{
    Task<CollabSession?> GetByIdAsync(Guid id);
    Task<List<CollabSession>> GetSessionsWhereUserIsParticipantAsync(Guid userId);
    Task<List<CollabSession>> GetSessionsCreatedByUserAsync(Guid userId);
    
    Task<CollabSession?> GetSessionWithParticipantsAsync(Guid sessionId);
    
    Task<CollabSession> CreateSessionAsync(CollabSession session);
    
    Task<CollabSession> UpdateSessionAsync(CollabSession session, SessionEditHistory history);

    
}