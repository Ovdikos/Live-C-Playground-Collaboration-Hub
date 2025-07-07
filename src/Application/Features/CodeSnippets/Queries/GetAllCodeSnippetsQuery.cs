using Application.DTOs;
using MediatR;

namespace Application.Features.CodeSnippets.Queries;

public record GetAllCodeSnippetsQuery(Guid OwnerId) 
    : IRequest<IEnumerable<SnippetDto>>;