using Application.DTOs;
using Application.Features.CollabSessions.Command;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CollabSessions.Handler.CommandHandler;

public class EditSessionHandler : IRequestHandler<EditSessionCommand, CollabSessionDto>
{
    private readonly ICollabParticipantRepository _repo;
    private readonly IMapper _mapper;

    public EditSessionHandler(ICollabParticipantRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<CollabSessionDto> Handle(EditSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _repo.GetByIdAsync(request.SessionId);
        if (session == null)
            throw new Exception("Session not found");

        session.Name = request.Name;
        session.EditedAt = DateTime.UtcNow;

        var history = new SessionEditHistory
        {
            Id = Guid.NewGuid(),
            SessionId = session.Id,
            EditedByUserId = request.EditedByUserId,
            EditedAt = session.EditedAt.Value,
            Changes = request.Changes
        };

        var updated = await _repo.UpdateSessionAsync(session, history);
        return _mapper.Map<CollabSessionDto>(updated);
    }
}
