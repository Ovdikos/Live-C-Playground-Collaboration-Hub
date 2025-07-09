using Application.DTOs;
using MediatR;

namespace Application.Features.Auth.Queries;

public class LoginUserQuery : IRequest<UserDto>
{
    public LoginUserDto Dto { get; init; } = null!;
}