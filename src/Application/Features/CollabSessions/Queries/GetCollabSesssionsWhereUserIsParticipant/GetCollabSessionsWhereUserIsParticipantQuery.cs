using Application.DTOs.SessionDtos;
using MediatR;

namespace Application.Features.CollabSessions.Queries.GetCollabSesssionsWhereUserIsParticipant;

public record GetCollabSessionsWhereUserIsParticipantQuery(Guid UserId) : IRequest<List<CollabSessionDto>>;
