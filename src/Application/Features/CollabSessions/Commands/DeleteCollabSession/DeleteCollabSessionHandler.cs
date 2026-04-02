using Core.Interfaces;
using MediatR;

namespace Application.Features.CollabSessions.Commands.DeleteCollabSession;

public class DeleteCollabSessionHandler: IRequestHandler<DeleteCollabSessionCommand>
{
    private readonly ICollabParticipantSessionRepository _repo; 

    public DeleteCollabSessionHandler(ICollabParticipantSessionRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(DeleteCollabSessionCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteSessionIfOwnerAsync(request.SessionId, request.UserId);
    }
}