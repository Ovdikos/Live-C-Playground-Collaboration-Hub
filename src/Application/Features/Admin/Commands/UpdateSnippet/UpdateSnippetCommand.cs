using Application.DTOs.SnippetsDtos;
using MediatR;

namespace Application.Features.Admin.Commands.UpdateSnippet;

public record UpdateSnippetCommand(SnippetDto Snippet) : IRequest<bool>;
