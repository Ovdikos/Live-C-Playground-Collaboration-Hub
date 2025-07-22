using Application.DTOs.SnippetsDtos;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CodeSnippets.Queries.GetAllCodeSnippets;

public class GetAllCodeSnippetsHandler
    : IRequestHandler<GetAllCodeSnippetsQuery, IEnumerable<SnippetDto>>
{
    private readonly ICodeSnippetRepository _repo;
    private readonly IMapper _mapper;

    public GetAllCodeSnippetsHandler(ICodeSnippetRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SnippetDto>> Handle(
        GetAllCodeSnippetsQuery request, 
        CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(request.OwnerId);
        return _mapper.Map<IEnumerable<SnippetDto>>(entities);
    }
}