using Application.DTOs;
using Application.Features.CollabSessions.Query;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CollabSessions.Handler.QueryHandler;

public class GetSessionsWhereUserIsParticipantHandler : IRequestHandler<GetSessionsWhereUserIsParticipantQuery, List<CollabSessionDto>>
{
    private readonly ICollabParticipantRepository _repo;
    private readonly IMapper _mapper;

    public GetSessionsWhereUserIsParticipantHandler(ICollabParticipantRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<CollabSessionDto>> Handle(GetSessionsWhereUserIsParticipantQuery request, CancellationToken cancellationToken)
    {
        var sessions = await _repo.GetSessionsWhereUserIsParticipantAsync(request.UserId);
        return _mapper.Map<List<CollabSessionDto>>(sessions);
    }
}