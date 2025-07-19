using Application.DTOs;
using Application.Features.Auth.Queries;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Auth.Handlers;

public class GetAllSnippetsHandler : IRequestHandler<GetAllSnippetsQuery, List<SnippetDto>>
{

    private readonly IAdminRepository _repo;
    private readonly IMapper _mapper;

    public GetAllSnippetsHandler(IAdminRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }
    

    public async Task<List<SnippetDto>> Handle(GetAllSnippetsQuery request, CancellationToken cancellationToken)
    {
        
        var snippets = await _repo.GetAllSnippetsAsync(request.IsPublic);
        return _mapper.Map<List<SnippetDto>>(snippets);
        
    }
}