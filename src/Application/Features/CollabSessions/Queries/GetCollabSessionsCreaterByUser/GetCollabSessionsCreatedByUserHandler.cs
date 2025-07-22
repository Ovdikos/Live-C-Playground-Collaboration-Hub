using Application.DTOs.SessionDtos;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CollabSessions.Queries.GetCollabSessionsCreaterByUser;

public class GetCollabSessionsCreatedByUserHandler : IRequestHandler<GetCollabSessionsCreatedByUserQuery, List<CollabSessionDto>>
{
    private readonly ICollabParticipantRepository _repo;
    private readonly IMapper _mapper;

    public GetCollabSessionsCreatedByUserHandler(ICollabParticipantRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<CollabSessionDto>> Handle(GetCollabSessionsCreatedByUserQuery request, CancellationToken cancellationToken)
    {
        var sessions = await _repo.GetSessionsCreatedByUserAsync(request.UserId);
        return _mapper.Map<List<CollabSessionDto>>(sessions);
    }
}