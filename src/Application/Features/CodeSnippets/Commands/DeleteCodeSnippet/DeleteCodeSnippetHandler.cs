using Core.Interfaces;
using MediatR;

namespace Application.Features.CodeSnippets.Commands.DeleteCodeSnippet;

public class DeleteCodeSnippetHandler : IRequestHandler<DeleteCodeSnippetCommand>
{
    
    private readonly ICodeSnippetRepository _repo;

    public DeleteCodeSnippetHandler(ICodeSnippetRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(DeleteCodeSnippetCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(request.Id);
    }
}