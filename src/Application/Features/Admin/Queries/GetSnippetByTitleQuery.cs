using Application.DTOs;
using MediatR;

namespace Application.Features.Admin.Queries;

public record GetSnippetByTitleQuery(string Title) : IRequest<SnippetDto?>;
