using Application.DTOs.SnippetsDtos;
using MediatR;

namespace Application.Features.Admin.Queries.GetSnippetByTitle;

public record GetSnippetByTitleQuery(string Title) : IRequest<SnippetDto?>;
