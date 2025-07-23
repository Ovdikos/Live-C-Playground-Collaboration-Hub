using System.Security.Claims;
using Application.DTOs.UserDtos;
using Application.JwtToken;
using AutoMapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/user")]
public class UserProfileController : ControllerBase
{
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateUserDto dto,
        [FromServices] IUserRepository repo,
        [FromServices] IJwtTokenService jwt,
        [FromServices] IMapper mapper)
    {
        var userIdStr = User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userIdStr == null || !Guid.TryParse(userIdStr, out var userId))
            return Unauthorized();

        var user = await repo.GetByIdAsync(userId);
        if (user == null)
            return NotFound();

        if (dto.Username != user.Username && await repo.GetByUsernameOrEmailAsync(dto.Username) is not null)
            return BadRequest("Login already taken");
        if (dto.Email != user.Email && (await repo.GetAllAsync()).Any(u => u.Email == dto.Email))
            return BadRequest("Email already taken");

        user.Username = dto.Username;
        user.Email = dto.Email;

        if (!string.IsNullOrWhiteSpace(dto.Password))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        if (!string.IsNullOrWhiteSpace(dto.AvatarBase64))
        {
            var ext = ".png";
            var fileName = $"{user.Id}{ext}";
            var filePath = Path.Combine("wwwroot/avatars", fileName);
            var base64Data = dto.AvatarBase64.Substring(dto.AvatarBase64.IndexOf(",") + 1);
            var bytes = Convert.FromBase64String(base64Data);
            await System.IO.File.WriteAllBytesAsync(filePath, bytes);
            user.AvatarFileName = fileName;
        }

        await repo.UpdateAsync(user);

        var updatedDto = mapper.Map<UserDto>(user);
        var token = jwt.GenerateToken(updatedDto);

        return Ok(new { token, user = updatedDto });
    }
}
