using Application.DTOs;
using Application.Features.CollabSessions.Query;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CollabSessions.Handler.QueryHandler;

public class GetSessionDetailsHandler : IRequestHandler<GetSessionDetailsQuery, CollabSessionDto>
{
    private readonly ICollabParticipantRepository _repo;
    private readonly IMapper _mapper; 

    public GetSessionDetailsHandler(ICollabParticipantRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<CollabSessionDto> Handle(GetSessionDetailsQuery request, CancellationToken cancellationToken)
    {
        var session = await _repo.GetSessionWithParticipantsAsync(request.SessionId)
                      ?? throw new KeyNotFoundException("Session not found");

        return _mapper.Map<CollabSessionDto>(session);
    }
}
