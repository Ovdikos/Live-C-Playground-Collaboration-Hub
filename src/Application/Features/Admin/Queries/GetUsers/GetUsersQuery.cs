using Application.DTOs.UserDtos;
using MediatR;

namespace Application.Features.Admin.Queries.GetUsers;

public record GetUsersQuery(
    string? Search = null,
    DateTime? CreatedFrom = null,
    DateTime? CreatedTo = null,
    bool? IsAdmin = null,
    string? OrderBy = null,
    bool Desc = false
) : IRequest<List<UserDto>>;