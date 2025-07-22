using Application.DTOs.SnippetsDtos;
using MediatR;

namespace Application.Features.Auth.Queries.GetAllSnippets;

public record GetAllSnippetsQuery(bool? IsPublic) : IRequest<List<SnippetDto>>;