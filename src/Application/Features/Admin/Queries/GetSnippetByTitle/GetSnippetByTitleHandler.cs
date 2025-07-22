using Application.DTOs.SnippetsDtos;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Admin.Queries.GetSnippetByTitle;

public class GetSnippetByTitleHandler : IRequestHandler<GetSnippetByTitleQuery, SnippetDto?>
{
    private readonly IAdminRepository _repo;
    private readonly IMapper _mapper;
    public GetSnippetByTitleHandler(IAdminRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<SnippetDto?> Handle(GetSnippetByTitleQuery request, CancellationToken cancellationToken)
    {
        var snippet = await _repo.GetSnippetByTitleAsync(request.Title);
        return snippet == null ? null : _mapper.Map<SnippetDto>(snippet);
    }
}