using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auth.Commands;

public class RegisterUserCommand : IRequest<UserDto>
{
    public RegisterUserDto Dto { get; init; } = null!;
    public IFormFile? Avatar { get; set; }
}