using Application.Features.CodeSnippets.Queries;
using Application.Mapper;
using Core.Interfaces;
using FluentValidation;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Web.Components;

var builder = WebApplication.CreateBuilder(args);

// 1. Реєстрація всіх сервісів у DI-контейнері
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<LivePlaygroundDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Репозиторії
builder.Services.AddScoped<ICodeSnippetRepository, CodeSnippetRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<HttpClient>(sp =>
{
    var navManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navManager.BaseUri) };
});

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<GetAllCodeSnippetsQuery>());


var app = builder.Build();

// 2. Middleware та маршрути
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Minimal API endpoints
app.MapGet("/api/snippets", async (IMediator mediator, Guid ownerId) =>
{
    var result = await mediator.Send(new GetAllCodeSnippetsQuery(ownerId));
    return Results.Ok(result);
});


// Blazor серверна частина
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
