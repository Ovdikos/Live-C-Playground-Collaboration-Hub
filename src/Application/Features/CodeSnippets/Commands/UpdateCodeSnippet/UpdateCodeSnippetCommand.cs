using MediatR;

namespace Application.Features.CodeSnippets.Commands.UpdateCodeSnippet;

public class UpdateCodeSnippetCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public bool IsPublic { get; set; }
    
}