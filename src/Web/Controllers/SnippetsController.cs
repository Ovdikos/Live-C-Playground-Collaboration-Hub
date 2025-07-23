using Application.Features.CodeSnippets.Commands.CreateCodeSnippet;
using Application.Features.CodeSnippets.Commands.DeleteCodeSnippet;
using Application.Features.CodeSnippets.Commands.UpdateCodeSnippet;
using Application.Features.CodeSnippets.Queries.GetAllCodeSnippets;
using Application.Features.CodeSnippets.Queries.GetCodeSnippetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/snippets")]
public class SnippetsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromServices] IMediator mediator, [FromQuery] Guid ownerId)
    {
        var result = await mediator.Send(new GetAllCodeSnippetsQuery(ownerId));
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromServices] IMediator mediator, [FromRoute] Guid id)
    {
        var snippet = await mediator.Send(new GetCodeSnippetByIdQuery(id));
        return snippet is null ? NotFound() : Ok(snippet);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromServices] IMediator mediator, [FromBody] CreateCodeSnippetCommand cmd)
    {
        var id = await mediator.Send(cmd);
        return Created($"/api/snippets/{id}", id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Edit([FromServices] IMediator mediator, [FromRoute] Guid id, [FromBody] UpdateCodeSnippetCommand cmd)
    {
        if (id != cmd.Id)
            return BadRequest();
        await mediator.Send(cmd);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromServices] IMediator mediator, [FromRoute] Guid id)
    {
        await mediator.Send(new DeleteCodeSnippetCommand(id));
        return NoContent();
    }
}