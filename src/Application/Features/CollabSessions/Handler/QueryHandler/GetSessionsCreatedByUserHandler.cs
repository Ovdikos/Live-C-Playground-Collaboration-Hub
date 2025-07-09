using Application.DTOs;
using Application.Features.CollabSessions.Query;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CollabSessions.Handler.QueryHandler;

public class GetSessionsCreatedByUserHandler : IRequestHandler<GetSessionsCreatedByUserQuery, List<CollabSessionDto>>
{
    private readonly ICollabParticipantRepository _repo;
    private readonly IMapper _mapper;

    public GetSessionsCreatedByUserHandler(ICollabParticipantRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<CollabSessionDto>> Handle(GetSessionsCreatedByUserQuery request, CancellationToken cancellationToken)
    {
        var sessions = await _repo.GetSessionsCreatedByUserAsync(request.UserId);
        return _mapper.Map<List<CollabSessionDto>>(sessions);
    }
}