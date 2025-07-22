using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CodeSnippets.Commands.CreateCodeSnippet;

public class CreateCodeSnippetHandler : IRequestHandler<CreateCodeSnippetCommand, Guid>
{
    
    private readonly ICodeSnippetRepository _repo;

    public CreateCodeSnippetHandler(ICodeSnippetRepository repo)
    {
        _repo = repo;
    }


    public async Task<Guid> Handle(CreateCodeSnippetCommand request, CancellationToken cancellationToken)
    {

        var snippet = new CodeSnippet
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            OwnerId = request.OwnerId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            IsPublic = request.IsPublic
        };
        
        await _repo.AddAsync(snippet);
        return snippet.Id;

    }
}