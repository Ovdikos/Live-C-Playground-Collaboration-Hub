using Application.DTOs.SessionDtos;
using MediatR;

namespace Application.Features.CollabSessions.Queries.GetCollabSessionDetails;

public record GetCollabSessionDetailsQuery(Guid SessionId) : IRequest<CollabSessionDto>;
