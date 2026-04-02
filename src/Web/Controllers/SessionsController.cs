using Application.DTOs.SessionDtos;
using Application.Features.CollabSessions.Commands.CreateCollabSession;
using Application.Features.CollabSessions.Commands.EditCollabSession;
using Application.Features.CollabSessions.Commands.JoinCollabSession;
using Application.Features.CollabSessions.Commands.LeaveCollabSession;
using Application.Features.CollabSessions.Queries.GetCollabSessionsCreaterByUser;
using Application.Features.CollabSessions.Queries.GetCollabSesssionsWhereUserIsParticipant;
using Application.Features.CollabSessions.Queries.GetSessionHistory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using DeleteCollabSessionCommand = Application.Features.CollabSessions.Commands.DeleteCollabSession.DeleteCollabSessionCommand;
using GetCollabSessionDetailsQuery = Application.Features.CollabSessions.Queries.GetCollabSessionDetails.GetCollabSessionDetailsQuery;

namespace Web.Controllers;

[ApiController]
[Route("api/sessions")]
public class SessionsController : ControllerBase
{
    
    private readonly IMediator _mediator;

    public SessionsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
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
    public async Task<IActionResult> GetInfo([FromRoute] Guid id)
    {
        var session = await _mediator.Send(new GetCollabSessionDetailsQuery(id));
        
        if (session == null)
            return NotFound();

        return Ok(session);
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
    public async Task<IActionResult> GetHistory([FromRoute] Guid id)
    {
        var history = await _mediator.Send(new GetSessionHistoryQuery(id));
        return Ok(history);
    }

    [HttpPost("join")]
    public async Task<IActionResult> Join([FromBody] JoinSessionDto dto)
    {
        try
        {
            await _mediator.Send(new JoinCollabSessionCommand(dto));
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("leave")]
    public async Task<IActionResult> Leave([FromBody] LeaveSessionDto dto)
    {
        try
        {
            await _mediator.Send(new LeaveCollabSessionCommand(dto));
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, [FromQuery] Guid userId)
    {
        try
        {
            await _mediator.Send(new DeleteCollabSessionCommand(id, userId));
            return Ok();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
