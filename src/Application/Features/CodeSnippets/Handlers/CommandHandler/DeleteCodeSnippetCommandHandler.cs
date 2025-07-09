using Application.Features.CodeSnippets.Commands;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CodeSnippets.Handlers.CommandHandler;

public class DeleteCodeSnippetCommandHandler : IRequestHandler<DeleteCodeSnippetCommand>
{
    
    private readonly ICodeSnippetRepository _repo;

    public DeleteCodeSnippetCommandHandler(ICodeSnippetRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(DeleteCodeSnippetCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(request.Id);
    }
}