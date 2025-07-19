using Application.DTOs;
using MediatR;

namespace Application.Features.Auth.Queries;

public class LoginUserQuery : IRequest<LoginResultDto>
{
    public LoginUserDto Dto { get; init; } = null!;
}