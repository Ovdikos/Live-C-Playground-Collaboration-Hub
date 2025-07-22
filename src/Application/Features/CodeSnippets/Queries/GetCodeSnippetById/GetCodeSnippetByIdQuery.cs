using Application.DTOs.SnippetsDtos;
using MediatR;

namespace Application.Features.CodeSnippets.Queries.GetCodeSnippetById;

public record GetCodeSnippetByIdQuery(Guid Id) : IRequest<SnippetDto>;