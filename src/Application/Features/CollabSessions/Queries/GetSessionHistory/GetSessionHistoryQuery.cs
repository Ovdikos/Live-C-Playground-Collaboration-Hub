using Application.DTOs.SessionDtos;
using MediatR;

namespace Application.Features.CollabSessions.Queries.GetSessionHistory;

public record GetSessionHistoryQuery(Guid CollabSessionId) : IRequest<CollabSessionDto>;