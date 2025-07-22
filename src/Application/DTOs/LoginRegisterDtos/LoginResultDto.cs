using Application.DTOs.UserDtos;

namespace Application.DTOs.LoginRegisterDtos;

public class LoginResultDto
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public string? Token { get; set; }
    public UserDto? User { get; set; }
}