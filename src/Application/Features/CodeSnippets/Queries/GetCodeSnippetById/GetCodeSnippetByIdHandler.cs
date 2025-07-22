using Application.DTOs.SnippetsDtos;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CodeSnippets.Queries.GetCodeSnippetById;

public class GetCodeSnippetByIdHandler : IRequestHandler<GetCodeSnippetByIdQuery, SnippetDto?>
{
    
    private readonly ICodeSnippetRepository _repo;
    private readonly IMapper _mapper;

    public GetCodeSnippetByIdHandler(ICodeSnippetRepository repo, IMapper mapper)
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