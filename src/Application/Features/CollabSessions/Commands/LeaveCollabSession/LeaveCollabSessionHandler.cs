using Core.Interfaces;
using MediatR;

namespace Application.Features.CollabSessions.Commands.LeaveCollabSession;

public class LeaveCollabSessionHandler : IRequestHandler<LeaveCollabSessionCommand>
{
    
    private readonly ICollabParticipantSessionRepository _repo;

    public LeaveCollabSessionHandler(ICollabParticipantSessionRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(LeaveCollabSessionCommand request, CancellationToken cancellationToken)
    {
        await _repo.RemoveParticipantAsync(request.Dto.SessionId, request.Dto.UserId);
    }
}