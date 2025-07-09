using Core.Entities;

namespace Core.Interfaces;

public interface ICollabParticipantRepository
{
    Task<List<CollabSession>> GetSessionsWhereUserIsParticipantAsync(Guid userId);
    Task<List<CollabSession>> GetSessionsCreatedByUserAsync(Guid userId);
}