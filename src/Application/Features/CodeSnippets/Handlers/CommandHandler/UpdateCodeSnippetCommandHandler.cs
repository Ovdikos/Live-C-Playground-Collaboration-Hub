using Application.Features.CodeSnippets.Commands;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CodeSnippets.Handlers.CommandHandler;

public class UpdateCodeSnippetCommandHandler : IRequestHandler<UpdateCodeSnippetCommand, Unit>
{
    
    private readonly ICodeSnippetRepository _repo;

    public UpdateCodeSnippetCommandHandler(ICodeSnippetRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(UpdateCodeSnippetCommand request, CancellationToken cancellationToken)
    {
        
        var snippet = await _repo.GetByIdAsync(request.Id);
        if (snippet == null) throw new KeyNotFoundException("The snippet could not be found.");
        
        snippet.Title = request.Title;
        snippet.Content = request.Content;
        snippet.IsPublic = request.IsPublic;
        snippet.UpdatedAt = DateTime.Now;
        
        await _repo.UpdateAsync(snippet);
        return Unit.Value;
    }
}