using MediatR;

namespace Application.Features.Admin.Commands.DeleteSnippet;

public record DeleteSnippetCommand(Guid SnippetId) : IRequest<bool>;