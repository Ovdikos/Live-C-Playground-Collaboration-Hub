using Application.DTOs.SessionDtos;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CollabSessions.Commands.JoinCollabSession;

public class JoinCollabSessionHandler : IRequestHandler<JoinCollabSessionCommand>
{

    private readonly ICollabParticipantSessionRepository _repo;

    public JoinCollabSessionHandler(ICollabParticipantSessionRepository repo, IMapper mapper)
    {
        _repo = repo;
    }


    public async Task Handle(JoinCollabSessionCommand request, CancellationToken cancellationToken)
    {
        await _repo.AddParticipantByJoinCodeAsync(request.Dto.JoinCode, request.Dto.UserId);
    }
}