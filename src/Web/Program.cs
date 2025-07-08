using System.Text;
using Application.AuthService;
using Application.DTOs;
using Application.Features.Auth.Commands;
using Application.Features.Auth.Queries;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Registering services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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
app.UseAntiforgery();

// Minimal API endpoints

// REGISTER
app.MapPost("/api/auth/register", async (IMediator mediator, IJwtTokenService jwt, [FromBody] RegisterUserCommand dto) =>
{
    var user = await mediator.Send(dto); 
    var token = jwt.GenerateToken(user);
    return Results.Ok(new { token, user });
});

// LOGIN
app.MapPost("/api/auth/login", async (IMediator mediator, IJwtTokenService jwt, LoginUserDto dto) =>
{
    var user = await mediator.Send(new LoginUserQuery(dto));
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


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
