using Application.DTOs;
using MediatR;

namespace Application.Features.CollabSessions.Query;

public record GetSessionDetailsQuery(Guid SessionId) : IRequest<CollabSessionDto>;
