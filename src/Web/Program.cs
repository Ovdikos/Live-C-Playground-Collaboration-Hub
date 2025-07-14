using System.Text;
using Application.DTOs;
using Application.Features.Auth.Commands;
using Application.Features.Auth.Queries;
using Application.Features.CodeSnippets.Commands;
using Application.Features.CodeSnippets.Queries;
using Application.Features.CollabSessions.Command;
using Application.Features.CollabSessions.Query;
using Application.Mapper;
using Application.Services;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using FluentValidation;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Web.Components;
using Web.Pages;

var builder = WebApplication.CreateBuilder(args);

// Registering services
// builder.Services.AddRazorComponents()
     // .AddInteractiveServerComponents();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<LivePlaygroundDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories/Services
builder.Services.AddScoped<ICodeSnippetRepository, CodeSnippetRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<ICollabParticipantRepository, CollabParticipantRepository>();


// HTTP CLIENT
builder.Services.AddScoped<HttpClient>(sp =>
{
    var navManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navManager.BaseUri) };
});

// UserState Service
builder.Services.AddScoped<UserState>();

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<GetAllCodeSnippetsQuery>());




// Auth


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });





var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// Minimal API endpoints


// REGISTER
app.MapPost("/api/auth/register", async (HttpRequest req, IMediator mediator, IJwtTokenService jwt) =>
{
    var form = await req.ReadFormAsync();
    var username = form["Username"];
    var password = form["Password"];
    var email = form["Email"];

    var avatar = form.Files.GetFile("Avatar");
    string? avatarFileName = null;
    if (avatar != null)
    {
        avatarFileName = $"{Guid.NewGuid()}.png";
        var path = Path.Combine("wwwroot/avatars", avatarFileName);
        using var stream = File.Create(path);
        await avatar.CopyToAsync(stream);
    }

    var dto = new RegisterUserDto
    {
        Username = username!,
        Password = password!,
        Email = email!,
        AvatarUrl = avatarFileName 
    };

    var cmd = new RegisterUserCommand { Dto = dto, Avatar = avatar };
    var user = await mediator.Send(cmd);
    var token = jwt.GenerateToken(user);
    return Results.Ok(new { token, user });
});

// LOGIN
app.MapPost("/api/auth/login", async (
    [FromBody] LoginUserDto dto,
    IMediator mediator,
    IJwtTokenService jwt) =>
{
    var qry = new LoginUserQuery { Dto = dto };
    var user = await mediator.Send(qry);
    if (user is null) 
        return Results.Unauthorized();
    var token = jwt.GenerateToken(user);
    return Results.Ok(new { token, user });
});

//USER SNIPPET

// GET ALL SNIPPETS
app.MapGet("/api/snippets", async (IMediator mediator, Guid ownerId) =>
{
    var result = await mediator.Send(new GetAllCodeSnippetsQuery(ownerId));
    return Results.Ok(result);
});

//GET SNIPPET BY ID
app.MapGet("/api/snippets/{id:guid}", async (IMediator mediator, Guid id) =>
{
    var snippet = await mediator.Send(new GetCodeSnippetByIdQuery(id));
    return snippet is null ? Results.NotFound() : Results.Ok(snippet);
});

// CREATE
app.MapPost("/api/snippets", async (IMediator mediator, CreateCodeSnippetCommand cmd) =>
{
    var id = await mediator.Send(cmd);
    return Results.Created($"/api/snippets/{id}", id);
});

// EDIT
app.MapPut("/api/snippets/{id:guid}", async (IMediator mediator, Guid id, UpdateCodeSnippetCommand cmd) =>
{
    if (id != cmd.Id)
        return Results.BadRequest();
    await mediator.Send(cmd);
    return Results.NoContent();
});

// DELETE
app.MapDelete("/api/snippets/{id}", async (IMediator mediator, Guid id) =>
{
    await mediator.Send(new DeleteCodeSnippetCommand(id));
    return Results.NoContent();
});


// USER SESSION

// GET ALL WHERE USER IS
app.MapGet("/api/sessions/participating", async (IMediator mediator, Guid userId) =>
{
    try
    {
    var result = await mediator.Send(new GetSessionsWhereUserIsParticipantQuery(userId));
    return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

// GET OWNED
app.MapGet("/api/sessions/owned", async (IMediator mediator, Guid userId) =>
{
    try
    {
    var result = await mediator.Send(new GetSessionsCreatedByUserQuery(userId));
    return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

// GET INFO 

app.MapGet("/api/sessions/{id}", async (Guid id, LivePlaygroundDbContext db, IMapper mapper) =>
{
    var session = await db.CollabSessions
        .Include(x => x.CodeSnippet)
        .Include(x => x.Owner)
        .Include(x => x.EditHistories).ThenInclude(h => h.EditedByUser)
        .Include(x => x.Participants).ThenInclude(p => p.User)
        .FirstOrDefaultAsync(x => x.Id == id);

    if (session == null)
        return Results.NotFound();

    return Results.Ok(mapper.Map<CollabSessionDto>(session));
});



// CREATE

app.MapPost("/api/sessions", async (
    CreateSessionCommand command, IMediator mediator) =>
{
    var sessionDto = await mediator.Send(command);
    return Results.Ok(sessionDto);
});

// EDIT

app.MapPut("/api/sessions/{id}", async (Guid id, EditSessionCommand command, IMediator mediator) =>
{
    if (id != command.SessionId) return Results.BadRequest();
    var updated = await mediator.Send(command);
    return Results.Ok(updated);
});

// GET HISTORY CHANGES
app.MapGet("/api/sessions/{id}/history", async (
    Guid id, LivePlaygroundDbContext db, IMapper mapper) =>
{
    var history = await db.SessionEditHistories
        .Where(h => h.SessionId == id)
        .Include(h => h.EditedByUser)
        .OrderByDescending(h => h.EditedAt)
        .ToListAsync();

    return mapper.Map<List<SessionEditHistoryDto>>(history);
});


// JOIN SESSION

app.MapPost("/api/sessions/join", async (JoinSessionDto dto, LivePlaygroundDbContext db) =>
{
    var session = await db.CollabSessions
        .Include(s => s.Participants)
        .FirstOrDefaultAsync(s => s.JoinCode == dto.JoinCode);

    if (session == null)
        return Results.NotFound();

    if (session.Participants.Any(p => p.UserId == dto.UserId))
        return Results.BadRequest();

    db.CollabParticipants.Add(new CollabParticipant
    {
        Id = Guid.NewGuid(),
        SessionId = session.Id,
        UserId = dto.UserId,
        JoinedAt = DateTime.UtcNow
    });
    await db.SaveChangesAsync();
    return Results.Ok();
});


// LEAVE SESIION
app.MapPost("/api/sessions/leave", async (LeaveSessionDto dto, LivePlaygroundDbContext db) =>
{
    var participant = await db.CollabParticipants
        .FirstOrDefaultAsync(p => p.SessionId == dto.SessionId && p.UserId == dto.UserId);

    if (participant == null)
        return Results.NotFound();

    db.CollabParticipants.Remove(participant);
    await db.SaveChangesAsync();
    return Results.Ok();
});


// DELETE SESSION

app.MapDelete("/api/sessions/{id}", async (Guid id, Guid userId, LivePlaygroundDbContext db) =>
{
    var session = await db.CollabSessions
        .FirstOrDefaultAsync(s => s.Id == id);

    if (session == null)
        return Results.NotFound();

    if (session.OwnerId != userId)
        return Results.Forbid();

    db.CollabSessions.Remove(session);
    await db.SaveChangesAsync();
    return Results.Ok();
});



// USER PROFILE





// app.MapRazorComponents<App>()
     // .AddInteractiveServerRenderMode();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
