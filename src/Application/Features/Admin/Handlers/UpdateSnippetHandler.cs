using Application.Features.Admin.Commands;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Admin.Handlers;

public class UpdateSnippetHandler : IRequestHandler<UpdateSnippetCommand, bool>
{
    private readonly IAdminRepository _repo;
    private readonly IMapper _mapper;
    public UpdateSnippetHandler(IAdminRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateSnippetCommand request, CancellationToken cancellationToken)
    {
        var snippet = _mapper.Map<CodeSnippet>(request.Snippet);
        return await _repo.UpdateSnippetAsync(snippet);
    }
}
