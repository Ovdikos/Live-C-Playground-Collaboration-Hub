using Application.DTOs.SessionDtos;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CollabSessions.Queries.GetCollabSessionDetails;

public class GetCollabSessionDetailsHandler : IRequestHandler<GetCollabSessionDetailsQuery, CollabSessionDto>
{
    private readonly ICollabParticipantRepository _repo;
    private readonly IMapper _mapper; 

    public GetCollabSessionDetailsHandler(ICollabParticipantRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<CollabSessionDto> Handle(GetCollabSessionDetailsQuery request, CancellationToken cancellationToken)
    {
        var session = await _repo.GetSessionWithParticipantsAsync(request.SessionId)
                      ?? throw new KeyNotFoundException("SessionDtos not found");

        return _mapper.Map<CollabSessionDto>(session);
    }
}
