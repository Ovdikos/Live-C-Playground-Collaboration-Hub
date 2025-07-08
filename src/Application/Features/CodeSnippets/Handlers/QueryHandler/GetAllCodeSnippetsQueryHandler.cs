using Application.DTOs;
using Application.Features.CodeSnippets.Queries;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CodeSnippets.Handlers.QueryHandler;

public class GetAllCodeSnippetsQueryHandler
    : IRequestHandler<GetAllCodeSnippetsQuery, IEnumerable<SnippetDto>>
{
    private readonly ICodeSnippetRepository _repo;
    private readonly IMapper _mapper;

    public GetAllCodeSnippetsQueryHandler(ICodeSnippetRepository repo, IMapper mapper)
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