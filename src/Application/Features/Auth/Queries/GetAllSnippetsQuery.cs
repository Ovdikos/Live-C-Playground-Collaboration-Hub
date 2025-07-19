using Application.DTOs;
using MediatR;

namespace Application.Features.Auth.Queries;

public record GetAllSnippetsQuery(bool? IsPublic) : IRequest<List<SnippetDto>>;