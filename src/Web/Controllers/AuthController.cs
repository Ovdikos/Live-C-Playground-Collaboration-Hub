using Application.DTOs.LoginRegisterDtos;
using Application.Features.Auth.Commands.RegisterUser;
using Application.Features.Auth.Queries.LoginUser;
using Application.JwtToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromForm] RegisterUserDto dto,
        [FromForm] IFormFile? Avatar,
        [FromServices] IMediator mediator,
        [FromServices] IJwtTokenService jwt)
    {
        string? avatarFileName = null;
        if (Avatar != null)
        {
            avatarFileName = $"{Guid.NewGuid()}.png";
            var path = Path.Combine("wwwroot/avatars", avatarFileName);
            using var stream = System.IO.File.Create(path);
            await Avatar.CopyToAsync(stream);
        }

        dto.AvatarUrl = avatarFileName;

        var cmd = new RegisterUserCommand { Dto = dto, Avatar = Avatar };
        var user = await mediator.Send(cmd);
        var token = jwt.GenerateToken(user);
        return Ok(new { token, user });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserDto dto,
        [FromServices] IMediator mediator,
        [FromServices] IJwtTokenService jwt)
    {
        var qry = new LoginUserQuery { Dto = dto };
        var result = await mediator.Send(qry);

        if (!result.Success)
            return BadRequest(new { error = result.Error });

        var token = jwt.GenerateToken(result.User!);
        result.Token = token;
        return Ok(result);
    }
}