using Application.DTOs;
using Core.Entities;

namespace Application.Services;

public interface IJwtTokenService
{
    string GenerateToken(UserDto user);
}