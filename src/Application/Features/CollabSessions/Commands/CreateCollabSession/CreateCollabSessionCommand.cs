using Application.DTOs.SessionDtos;
using MediatR;

namespace Application.Features.CollabSessions.Commands.CreateCollabSession;

public record CreateSessionCommand(string Name, Guid CodeSnippetId, Guid OwnerId, string JoinCode) : IRequest<CollabSessionDto>;
