using Application.DTOs;
using Application.Features.Admin.Queries;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Admin.Handlers;

public class GetCollabSessionDetailsHandler : IRequestHandler<GetCollabSessionDetailsQuery, CollabSessionDetailsDto?>
{
    private readonly IAdminRepository _repo;
    private readonly IMapper _mapper;

    public GetCollabSessionDetailsHandler(IAdminRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<CollabSessionDetailsDto?> Handle(GetCollabSessionDetailsQuery request, CancellationToken cancellationToken)
    {
        var session = await _repo.GetByNameAsync(request.Name);
        return session == null ? null : _mapper.Map<CollabSessionDetailsDto>(session);
    }
}