using Application.DTOs.SessionDtos;
using Application.Features.CollabSessions.Commands.CreateCollabSession;
using Application.Features.CollabSessions.Commands.EditCollabSession;
using Application.Features.CollabSessions.Queries.GetCollabSessionsCreaterByUser;
using Application.Features.CollabSessions.Queries.GetCollabSesssionsWhereUserIsParticipant;
using AutoMapper;
using Core.Entities;
using Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers;

[ApiController]
[Route("api/sessions")]
public class SessionsController : ControllerBase
{
    [HttpGet("participating")]
    public async Task<IActionResult> GetParticipating([FromServices] IMediator mediator, [FromQuery] Guid userId)
    {
        try
        {
            var result = await mediator.Send(new GetCollabSessionsWhereUserIsParticipantQuery(userId));
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("owned")]
    public async Task<IActionResult> GetOwned([FromServices] IMediator mediator, [FromQuery] Guid userId)
    {
        try
        {
            var result = await mediator.Send(new GetCollabSessionsCreatedByUserQuery(userId));
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInfo([FromRoute] Guid id, [FromServices] LivePlaygroundDbContext db, [FromServices] IMapper mapper)
    {
        var session = await db.CollabSessions
            .Include(x => x.CodeSnippet)
            .Include(x => x.Owner)
            .Include(x => x.EditHistories).ThenInclude(h => h.EditedByUser)
            .Include(x => x.Participants).ThenInclude(p => p.User)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (session == null)
            return NotFound();

        return Ok(mapper.Map<CollabSessionDto>(session));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSessionCommand command, [FromServices] IMediator mediator)
    {
        var sessionDto = await mediator.Send(command);
        return Ok(sessionDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Edit([FromRoute] Guid id, [FromBody] EditCollabSessionCommand command, [FromServices] IMediator mediator)
    {
        if (id != command.SessionId) return BadRequest();
        var updated = await mediator.Send(command);
        return Ok(updated);
    }

    [HttpGet("{id}/history")]
    public async Task<IActionResult> GetHistory([FromRoute] Guid id, [FromServices] LivePlaygroundDbContext db, [FromServices] IMapper mapper)
    {
        var history = await db.SessionEditHistories
            .Where(h => h.SessionId == id)
            .Include(h => h.EditedByUser)
            .OrderByDescending(h => h.EditedAt)
            .ToListAsync();

        return Ok(mapper.Map<List<SessionEditHistoryDto>>(history));
    }

    [HttpPost("join")]
    public async Task<IActionResult> Join([FromBody] JoinSessionDto dto, [FromServices] LivePlaygroundDbContext db)
    {
        var session = await db.CollabSessions
            .Include(s => s.Participants)
            .FirstOrDefaultAsync(s => s.JoinCode == dto.JoinCode);

        if (session == null)
            return NotFound();

        if (session.Participants.Any(p => p.UserId == dto.UserId))
            return BadRequest();

        db.CollabParticipants.Add(new CollabParticipant
        {
            Id = Guid.NewGuid(),
            SessionId = session.Id,
            UserId = dto.UserId,
            JoinedAt = DateTime.UtcNow
        });
        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("leave")]
    public async Task<IActionResult> Leave([FromBody] LeaveSessionDto dto, [FromServices] LivePlaygroundDbContext db)
    {
        var participant = await db.CollabParticipants
            .FirstOrDefaultAsync(p => p.SessionId == dto.SessionId && p.UserId == dto.UserId);

        if (participant == null)
            return NotFound();

        db.CollabParticipants.Remove(participant);
        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, [FromQuery] Guid userId, [FromServices] LivePlaygroundDbContext db)
    {
        var session = await db.CollabSessions
            .FirstOrDefaultAsync(s => s.Id == id);

        if (session == null)
            return NotFound();

        if (session.OwnerId != userId)
            return Forbid();

        db.CollabSessions.Remove(session);
        await db.SaveChangesAsync();
        return Ok();
    }
}
