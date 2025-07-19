using Application.DTOs;
using Application.Features.Auth.Queries;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Auth.Handlers;

public class GetAllSessionsHandler : IRequestHandler<GetAllSessionsQuery, List<CollabSessionDetailsDto>>
{
 
    private readonly IAdminRepository _repo;
    private readonly IMapper _mapper;
    public GetAllSessionsHandler(IAdminRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<CollabSessionDetailsDto>> Handle(GetAllSessionsQuery request, CancellationToken ct)
    {
        var sessions = await _repo.GetAllSessionsAsync(request.Search, request.IsActive, request.CreatedFrom, request.CreatedTo, request.MinParticipants);
        return _mapper.Map<List<CollabSessionDetailsDto>>(sessions);
    }
    
}