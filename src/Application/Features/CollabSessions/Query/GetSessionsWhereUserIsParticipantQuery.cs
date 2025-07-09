using Application.DTOs;
using MediatR;

namespace Application.Features.CollabSessions.Query;

public record GetSessionsWhereUserIsParticipantQuery(Guid UserId) : IRequest<List<CollabSessionDto>>;
