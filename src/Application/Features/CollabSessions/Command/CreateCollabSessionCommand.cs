using Application.DTOs;
using MediatR;

namespace Application.Features.CollabSessions.Command;

public record CreateSessionCommand(string Name, Guid CodeSnippetId, Guid OwnerId, string JoinCode) : IRequest<CollabSessionDto>;
