using Application.DTOs.LoginRegisterDtos;
using MediatR;

namespace Application.Features.Auth.Queries.LoginUser;

public class LoginUserQuery : IRequest<LoginResultDto>
{
    public LoginUserDto Dto { get; init; } = null!;
}