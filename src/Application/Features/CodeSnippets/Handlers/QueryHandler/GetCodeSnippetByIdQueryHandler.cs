using Application.DTOs;
using Application.Features.CodeSnippets.Queries;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CodeSnippets.Handlers.QueryHandler;

public class GetCodeSnippetByIdQueryHandler : IRequestHandler<GetCodeSnippetByIdQuery, SnippetDto?>
{
    
    private readonly ICodeSnippetRepository _repo;
    private readonly IMapper _mapper;

    public GetCodeSnippetByIdQueryHandler(ICodeSnippetRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<SnippetDto?> Handle(GetCodeSnippetByIdQuery request, CancellationToken cancellationToken)
    {
        var snippet = await _repo.GetByIdAsync(request.Id);
        return snippet is null ? null : _mapper.Map<SnippetDto>(snippet);
    }
}