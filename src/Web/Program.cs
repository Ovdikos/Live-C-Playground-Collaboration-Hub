using System.Text;
using Application.AuthService;
using Application.DTOs;
using Application.Features.Auth.Commands;
using Application.Features.Auth.Queries;
using Application.Features.CodeSnippets.Commands;
using Application.Features.CodeSnippets.Queries;
using Application.Mapper;
using Application.Services;
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


// app.MapGet("/test/users/exists", async ([FromQuery] string username, [FromQuery] string email, IUserRepository repo) =>
// {
//     var exists = await repo.ExistsByUsernameOrEmail(username, email);
//     return Results.Ok(new { exists });
// });
//
// app.MapPost("/test/users/add", async ([FromBody] RegisterUserDto dto, IUserRepository repo) =>
// {
//     var user = new Core.Entities.User
//     {
//         Username     = dto.Username,
//         Email        = dto.Email,
//         PasswordHash = "dummy",   // тимчасово
//         CreatedAt    = DateTime.UtcNow,
//         IsAdmin      = false
//     };
//     await repo.AddAsync(user);
//     return Results.Created($"/test/users/{user.Id}", user);
// });


// REGISTER
app.MapPost("/api/auth/register", async (
    [FromBody] RegisterUserDto dto,
    IMediator mediator,
    IJwtTokenService jwt) =>
{
    var cmd = new RegisterUserCommand { Dto = dto };
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


// app.MapRazorComponents<App>()
     // .AddInteractiveServerRenderMode();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
