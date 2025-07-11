using Application.DTOs;
using Application.Features.CollabSessions.Command;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CollabSessions.Handler.CommandHandler;

public class CreateCollabSessionCommandHandler : IRequestHandler<CreateSessionCommand, CollabSessionDto>
{
    private readonly ICollabParticipantRepository _repo;
    private readonly IMapper _mapper;

    public CreateCollabSessionCommandHandler(ICollabParticipantRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<CollabSessionDto> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var session = new CollabSession
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            OwnerId = request.OwnerId,
            CodeSnippetId = request.CodeSnippetId,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            JoinCode = request.JoinCode
        };

        var result = await _repo.CreateSessionAsync(session);
        return _mapper.Map<CollabSessionDto>(result);
    }
}