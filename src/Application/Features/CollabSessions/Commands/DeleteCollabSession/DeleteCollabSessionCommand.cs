using MediatR;

namespace Application.Features.CollabSessions.Commands.DeleteCollabSession;

public record DeleteCollabSessionCommand(Guid SessionId, Guid UserId) : IRequest;
