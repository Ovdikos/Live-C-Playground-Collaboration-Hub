using Application.DTOs;
using MediatR;

namespace Application.Features.CodeSnippets.Queries;

public record GetCodeSnippetByIdQuery(Guid Id) : IRequest<SnippetDto>;