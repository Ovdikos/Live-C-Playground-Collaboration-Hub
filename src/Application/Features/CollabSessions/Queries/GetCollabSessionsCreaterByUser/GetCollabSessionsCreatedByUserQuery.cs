using Application.DTOs.SessionDtos;
using MediatR;

namespace Application.Features.CollabSessions.Queries.GetCollabSessionsCreaterByUser;

public record GetCollabSessionsCreatedByUserQuery(Guid UserId) : IRequest<List<CollabSessionDto>>;
