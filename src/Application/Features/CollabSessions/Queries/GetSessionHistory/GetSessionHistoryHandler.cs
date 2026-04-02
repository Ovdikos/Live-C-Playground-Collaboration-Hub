using Application.DTOs.SessionDtos;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CollabSessions.Queries.GetSessionHistory;

public class GetSessionHistoryHandler : IRequestHandler<GetSessionHistoryQuery, CollabSessionDto>
{
    
    private readonly ICollabParticipantSessionRepository _repo;
    private readonly IMapper _mapper;

    public GetSessionHistoryHandler(ICollabParticipantSessionRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }


    public async Task<CollabSessionDto> Handle(GetSessionHistoryQuery request, CancellationToken cancellationToken)
    {
        var session = await _repo.GetCollabSessionHistoryAsync(request.CollabSessionId)
                ?? throw new KeyNotFoundException("Error of showing a session history");
        
        return _mapper.Map<CollabSessionDto>(session);
    }
    
}
    