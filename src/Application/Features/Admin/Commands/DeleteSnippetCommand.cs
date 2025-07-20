using MediatR;

namespace Application.Features.Admin.Commands;

public record DeleteSnippetCommand(Guid SnippetId) : IRequest<bool>;