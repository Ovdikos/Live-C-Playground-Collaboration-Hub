using Application.DTOs;
using MediatR;

namespace Application.Features.Admin.Commands;

public record UpdateSnippetCommand(SnippetDto Snippet) : IRequest<bool>;
