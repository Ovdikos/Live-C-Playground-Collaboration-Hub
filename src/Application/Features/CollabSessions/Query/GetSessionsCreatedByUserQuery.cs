using Application.DTOs;
using MediatR;

namespace Application.Features.CollabSessions.Query;

public record GetSessionsCreatedByUserQuery(Guid UserId) : IRequest<List<CollabSessionDto>>;
