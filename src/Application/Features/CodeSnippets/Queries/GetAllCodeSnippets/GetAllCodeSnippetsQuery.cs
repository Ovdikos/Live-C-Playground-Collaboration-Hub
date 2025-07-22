using Application.DTOs.SnippetsDtos;
using MediatR;

namespace Application.Features.CodeSnippets.Queries.GetAllCodeSnippets;

public record GetAllCodeSnippetsQuery(Guid OwnerId) 
    : IRequest<IEnumerable<SnippetDto>>;