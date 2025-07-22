using Application.DTOs.UserDtos;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Admin.Queries.GetUsers;

public class GetUsersHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    private readonly IAdminRepository _repo;
    public GetUsersHandler(IAdminRepository repo) { _repo = repo; }

    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _repo.GetUsersAsync(
            request.Search, request.CreatedFrom, request.CreatedTo, request.IsAdmin, request.OrderBy, request.Desc
        );

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            CreatedAt = u.CreatedAt,
            IsAdmin = u.IsAdmin,
            AvatarFileName = u.AvatarFileName
        }).ToList();
    }
}