using Application.Features.Admin.Commands;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Admin.Handlers;

public class DeleteSnippetHandler : IRequestHandler<DeleteSnippetCommand, bool>
{
    private readonly IAdminRepository _repo;
    public DeleteSnippetHandler(IAdminRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteSnippetCommand request, CancellationToken cancellationToken)
    {
        return await _repo.DeleteSnippetAsync(request.SnippetId);
    }
}