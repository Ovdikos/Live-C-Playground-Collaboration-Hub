using Application.DTOs;
using MediatR;

namespace Application.Features.Auth.Commands;

public class RegisterUserCommand : IRequest<UserDto>
{
    // public string Username { get; set; } = string.Empty;
    // public string Password { get; set; } = string.Empty;
    // public string Email { get; set; } = string.Empty;
    // public string? AvatarUrl { get; set; }
    
    public RegisterUserDto Dto { get; init; } = null!;
}