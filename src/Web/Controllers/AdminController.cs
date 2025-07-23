using System.Security.Claims;
using Application.DTOs.SessionDtos;
using Application.DTOs.SnippetsDtos;
using Application.Features.Admin.Commands.BlockUser;
using Application.Features.Admin.Commands.DeleteCollabSession;
using Application.Features.Admin.Commands.DeleteSnippet;
using Application.Features.Admin.Commands.DeleteUser;
using Application.Features.Admin.Commands.UpdateCollabSession;
using Application.Features.Admin.Commands.UpdateSnippet;
using Application.Features.Admin.Queries.GetCollabSessionDetails;
using Application.Features.Admin.Queries.GetSnippetByTitle;
using Application.Features.Admin.Queries.GetUserDetails;
using Application.Features.Admin.Queries.GetUsers;
using Application.Features.Auth.Queries.GetAllSessions;
using Application.Features.Auth.Queries.GetAllSnippets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private bool IsAdmin() => User.Claims.Any(c => c.Type == "isAdmin" && c.Value == "True");

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query, [FromServices] IMediator mediator)
    {
        if (!IsAdmin()) return Forbid();
        var users = await mediator.Send(query);
        return Ok(users);
    }

    [HttpGet("user/{username}")]
    public async Task<IActionResult> GetUserByUsername([FromRoute] string username, [FromServices] IMediator mediator)
    {
        if (!IsAdmin()) return Forbid();

        var userDetails = await mediator.Send(new GetUserDetailsQuery(username));
        if (userDetails == null)
            return NotFound();

        return Ok(userDetails);
    }

    [HttpPost("user/block")]
    public async Task<IActionResult> BlockUser([FromBody] BlockUserCommand cmd, [FromServices] IMediator mediator)
    {
        if (!IsAdmin()) return Forbid();

        var adminEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var result = await mediator.Send(cmd with { AdminEmail = adminEmail });
        return result ? Ok() : NotFound();
    }

    [HttpDelete("user/{userId}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId, [FromServices] IMediator mediator)
    {
        if (!IsAdmin()) return Forbid();

        var result = await mediator.Send(new DeleteUserCommand(userId));
        return result ? Ok() : NotFound();
    }

    [HttpGet("snippets")]
    public async Task<IActionResult> GetAllSnippets([FromQuery] bool? isPublic, [FromServices] IMediator mediator)
    {
        if (!IsAdmin()) return Forbid();

        var result = await mediator.Send(new GetAllSnippetsQuery(isPublic));
        return Ok(result);
    }

    [HttpGet("sessions")]
    public async Task<IActionResult> GetAllSessions(
        [FromQuery] string? search,
        [FromQuery] bool? isActive,
        [FromQuery] DateTime? createdFrom,
        [FromQuery] DateTime? createdTo,
        [FromQuery] int? minParticipants,
        [FromServices] IMediator mediator)
    {
        if (!IsAdmin()) return Forbid();

        var sessions = await mediator.Send(new GetAllSessionsQuery(search, isActive, createdFrom, createdTo, minParticipants));
        return Ok(sessions);
    }

    [HttpGet("snippet")]
    public async Task<IActionResult> FindSnippetByTitle([FromQuery] string title, [FromServices] IMediator mediator)
    {
        if (!IsAdmin()) return Forbid();
        var snippet = await mediator.Send(new GetSnippetByTitleQuery(title));
        if (snippet == null)
            return NotFound();
        return Ok(snippet);
    }

    [HttpPut("snippet")]
    public async Task<IActionResult> UpdateSnippet([FromBody] SnippetDto snippet, [FromServices] IMediator mediator)
    {
        if (!IsAdmin()) return Forbid();
        var result = await mediator.Send(new UpdateSnippetCommand(snippet));
        if (!result) return BadRequest();
        return Ok();
    }

    [HttpDelete("snippet/{id}")]
    public async Task<IActionResult> DeleteSnippet([FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        if (!IsAdmin()) return Forbid();

        var deleted = await mediator.Send(new DeleteSnippetCommand(id));
        if (!deleted) return NotFound();

        return Ok();
    }

    [HttpGet("session")]
    public async Task<IActionResult> GetSessionByName([FromQuery] string name, [FromServices] IMediator mediator)
    {
        if (!IsAdmin()) return Forbid();

        var session = await mediator.Send(new GetCollabSessionDetailsQuery(name));
        if (session == null) return NotFound();

        return Ok(session);
    }

    [HttpPut("session")]
    public async Task<IActionResult> UpdateSession([FromBody] CollabSessionEditDto dto, [FromServices] IMediator mediator)
    {
        if (!IsAdmin()) return Forbid();
        var updated = await mediator.Send(new UpdateCollabSessionCommand(dto));
        return updated ? Ok() : BadRequest();
    }

    [HttpDelete("session/{name}")]
    public async Task<IActionResult> DeleteSession([FromRoute] string name, [FromServices] IMediator mediator)
    {
        if (!IsAdmin()) return Forbid();

        var deleted = await mediator.Send(new DeleteCollabSessionCommand(name));
        if (!deleted) return NotFound();

        return Ok();
    }
}
