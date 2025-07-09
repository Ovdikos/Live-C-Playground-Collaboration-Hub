using System.Windows.Input;
using MediatR;

namespace Application.Features.CodeSnippets.Commands;

public record DeleteCodeSnippetCommand(Guid Id) : IRequest;