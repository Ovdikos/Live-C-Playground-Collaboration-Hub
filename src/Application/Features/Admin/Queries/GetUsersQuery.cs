using Application.DTOs;
using MediatR;

namespace Application.Features.Admin.Queries;

public record GetUsersQuery(
    string? Search = null,
    DateTime? CreatedFrom = null,
    DateTime? CreatedTo = null,
    bool? IsAdmin = null,
    string? OrderBy = null,
    bool Desc = false
) : IRequest<List<UserDto>>;