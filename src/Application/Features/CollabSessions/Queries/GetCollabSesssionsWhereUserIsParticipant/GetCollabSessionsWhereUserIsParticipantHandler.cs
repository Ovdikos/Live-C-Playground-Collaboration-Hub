using Application.DTOs.SessionDtos;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CollabSessions.Queries.GetCollabSesssionsWhereUserIsParticipant;

public class GetCollabSessionsWhereUserIsParticipantHandler : IRequestHandler<GetCollabSessionsWhereUserIsParticipantQuery, List<CollabSessionDto>>
{
    private readonly ICollabParticipantRepository _repo;
    private readonly IMapper _mapper;

    public GetCollabSessionsWhereUserIsParticipantHandler(ICollabParticipantRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<CollabSessionDto>> Handle(GetCollabSessionsWhereUserIsParticipantQuery request, CancellationToken cancellationToken)
    {
        var sessions = await _repo.GetSessionsWhereUserIsParticipantAsync(request.UserId);
        return _mapper.Map<List<CollabSessionDto>>(sessions);
    }
}