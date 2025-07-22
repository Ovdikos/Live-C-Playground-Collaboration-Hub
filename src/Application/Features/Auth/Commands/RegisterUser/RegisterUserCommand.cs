using Application.DTOs.LoginRegisterDtos;
using Application.DTOs.UserDtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auth.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<UserDto>
{
    public RegisterUserDto Dto { get; init; } = null!;
    public IFormFile? Avatar { get; set; }
}