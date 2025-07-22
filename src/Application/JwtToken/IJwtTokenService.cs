using Application.DTOs.UserDtos;

namespace Application.JwtToken;

public interface IJwtTokenService
{
    string GenerateToken(UserDto user);
}