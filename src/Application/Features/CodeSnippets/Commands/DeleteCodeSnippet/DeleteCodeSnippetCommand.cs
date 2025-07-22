using MediatR;

namespace Application.Features.CodeSnippets.Commands.DeleteCodeSnippet;

public record DeleteCodeSnippetCommand(Guid Id) : IRequest;