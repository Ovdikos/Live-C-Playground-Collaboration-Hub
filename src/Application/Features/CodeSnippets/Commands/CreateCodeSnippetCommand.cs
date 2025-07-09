using MediatR;

namespace Application.Features.CodeSnippets.Commands;

public class CreateCodeSnippetCommand : IRequest<Guid>
{
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public Guid OwnerId { get; set; }
    public bool IsPublic { get; set; }
}