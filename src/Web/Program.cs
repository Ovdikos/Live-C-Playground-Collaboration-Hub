using System.Text;
using Application.Features.Admin.CleanupService;
using Application.Features.CodeSnippets.Queries.GetAllCodeSnippets;
using Application.JwtToken;
using Application.Mapper;
using Core.Interfaces;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Web.AuthService;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Registering services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<LivePlaygroundDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories/JwtToken
builder.Services.AddScoped<ICodeSnippetRepository, CodeSnippetRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<ICollabParticipantRepository, CollabParticipantRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();


// HTTP CLIENT
builder.Services.AddScoped<HttpClient>(sp =>
{
    var navManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navManager.BaseUri) };
});

// Auth Service
builder.Services.AddScoped<UserState>();
builder.Services.AddScoped<LocalStorageService>();

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<GetAllCodeSnippetsQuery>());

// Hosted Service
builder.Services.AddHostedService<SessionDeactivationService>();



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


// Controllers

builder.Services.AddControllers();


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

// // Minimal API endpoints
//
//
// // REGISTER
// app.MapPost("/api/auth/register", async (HttpRequest req, IMediator mediator, IJwtTokenService jwt) =>
// {
//     var form = await req.ReadFormAsync();
//     var username = form["Login"];
//     var password = form["Password"];
//     var email = form["Email"];
//
//     var avatar = form.Files.GetFile("Avatar");
//     string? avatarFileName = null;
//     if (avatar != null)
//     {
//         avatarFileName = $"{Guid.NewGuid()}.png";
//         var path = Path.Combine("wwwroot/avatars", avatarFileName);
//         using var stream = File.Create(path);
//         await avatar.CopyToAsync(stream);
//     }
//
//     var dto = new RegisterUserDto
//     {
//         Login = username!,
//         Password = password!,
//         Email = email!,
//         AvatarUrl = avatarFileName 
//     };
//
//     var cmd = new RegisterUserCommand { Dto = dto, Avatar = avatar };
//     var user = await mediator.Send(cmd);
//     var token = jwt.GenerateToken(user);
//     return Results.Ok(new { token, user });
// });
//
// // LOGIN
// app.MapPost("/api/auth/login", async (
//     [FromBody] LoginUserDto dto,
//     IMediator mediator,
//     IJwtTokenService jwt
// ) =>
// {
//     var qry = new LoginUserQuery { Dto = dto };
//     var result = await mediator.Send(qry);
//
//     if (!result.Success)
//         return Results.BadRequest(new { error = result.Error });
//
//     var token = jwt.GenerateToken(result.User!);
//     result.Token = token;
//     return Results.Ok(result);
// });
//
// //USER SNIPPET
//
// // GET ALL SNIPPETS
// app.MapGet("/api/snippets", async (IMediator mediator, Guid ownerId) =>
// {
//     var result = await mediator.Send(new GetAllCodeSnippetsQuery(ownerId));
//     return Results.Ok(result);
// });
//
// //GET SNIPPET BY ID
// app.MapGet("/api/snippets/{id:guid}", async (IMediator mediator, Guid id) =>
// {
//     var snippet = await mediator.Send(new GetCodeSnippetByIdQuery(id));
//     return snippet is null ? Results.NotFound() : Results.Ok(snippet);
// });
//
// // CREATE
// app.MapPost("/api/snippets", async (IMediator mediator, CreateCodeSnippetCommand cmd) =>
// {
//     var id = await mediator.Send(cmd);
//     return Results.Created($"/api/snippets/{id}", id);
// });
//
// // EDIT
// app.MapPut("/api/snippets/{id:guid}", async (IMediator mediator, Guid id, UpdateCodeSnippetCommand cmd) =>
// {
//     if (id != cmd.Id)
//         return Results.BadRequest();
//     await mediator.Send(cmd);
//     return Results.NoContent();
// });
//
// // DELETE
// app.MapDelete("/api/snippets/{id}", async (IMediator mediator, Guid id) =>
// {
//     await mediator.Send(new DeleteCodeSnippetCommand(id));
//     return Results.NoContent();
// });
//
//
// // USER SESSION
//
// // GET ALL WHERE USER IS
// app.MapGet("/api/sessions/participating", async (IMediator mediator, Guid userId) =>
// {
//     try
//     {
//         var result = await mediator.Send(new GetCollabSessionsWhereUserIsParticipantQuery(userId));
//         return Results.Ok(result);
//     }
//     catch (Exception ex)
//     {
//         return Results.BadRequest(ex.Message);
//     }
// });
//
// // GET OWNED
// app.MapGet("/api/sessions/owned", async (IMediator mediator, Guid userId) =>
// {
//     try
//     {
//         var result = await mediator.Send(new GetCollabSessionsCreatedByUserQuery(userId));
//         return Results.Ok(result);
//     }
//     catch (Exception ex)
//     {
//         return Results.BadRequest(ex.Message);
//     }
// });
//
// // GET INFO 
//
// app.MapGet("/api/sessions/{id}", async (Guid id, LivePlaygroundDbContext db, IMapper mapper) =>
// {
//     var session = await db.CollabSessions
//         .Include(x => x.CodeSnippet)
//         .Include(x => x.Owner)
//         .Include(x => x.EditHistories).ThenInclude(h => h.EditedByUser)
//         .Include(x => x.Participants).ThenInclude(p => p.User)
//         .FirstOrDefaultAsync(x => x.Id == id);
//
//     if (session == null)
//         return Results.NotFound();
//
//     return Results.Ok(mapper.Map<CollabSessionDto>(session));
// });
//
//
//
// // CREATE
//
// app.MapPost("/api/sessions", async (
//     CreateSessionCommand command, IMediator mediator) =>
// {
//     var sessionDto = await mediator.Send(command);
//     return Results.Ok(sessionDto);
// });
//
// // EDIT
//
// app.MapPut("/api/sessions/{id}", async (Guid id, EditCollabSessionCommand command, IMediator mediator) =>
// {
//     if (id != command.SessionId) return Results.BadRequest();
//     var updated = await mediator.Send(command);
//     return Results.Ok(updated);
// });
//
// // GET HISTORY CHANGES
// app.MapGet("/api/sessions/{id}/history", async (
//     Guid id, LivePlaygroundDbContext db, IMapper mapper) =>
// {
//     var history = await db.SessionEditHistories
//         .Where(h => h.SessionId == id)
//         .Include(h => h.EditedByUser)
//         .OrderByDescending(h => h.EditedAt)
//         .ToListAsync();
//
//     return mapper.Map<List<SessionEditHistoryDto>>(history);
// });
//
//
// // JOIN SESSION
//
// app.MapPost("/api/sessions/join", async (JoinSessionDto dto, LivePlaygroundDbContext db) =>
// {
//     var session = await db.CollabSessions
//         .Include(s => s.Participants)
//         .FirstOrDefaultAsync(s => s.JoinCode == dto.JoinCode);
//
//     if (session == null)
//         return Results.NotFound();
//
//     if (session.Participants.Any(p => p.UserId == dto.UserId))
//         return Results.BadRequest();
//
//     db.CollabParticipants.Add(new CollabParticipant
//     {
//         Id = Guid.NewGuid(),
//         SessionId = session.Id,
//         UserId = dto.UserId,
//         JoinedAt = DateTime.UtcNow
//     });
//     await db.SaveChangesAsync();
//     return Results.Ok();
// });
//
//
// // LEAVE SESIION
// app.MapPost("/api/sessions/leave", async (LeaveSessionDto dto, LivePlaygroundDbContext db) =>
// {
//     var participant = await db.CollabParticipants
//         .FirstOrDefaultAsync(p => p.SessionId == dto.SessionId && p.UserId == dto.UserId);
//
//     if (participant == null)
//         return Results.NotFound();
//
//     db.CollabParticipants.Remove(participant);
//     await db.SaveChangesAsync();
//     return Results.Ok();
// });
//
//
// // DELETE SESSION
//
// app.MapDelete("/api/sessions/{id}", async (Guid id, Guid userId, LivePlaygroundDbContext db) =>
// {
//     var session = await db.CollabSessions
//         .FirstOrDefaultAsync(s => s.Id == id);
//
//     if (session == null)
//         return Results.NotFound();
//
//     if (session.OwnerId != userId)
//         return Results.Forbid();
//
//     db.CollabSessions.Remove(session);
//     await db.SaveChangesAsync();
//     return Results.Ok();
// });
//
//
//
// // USER PROFILE
//
// app.MapPut("/api/user/profile", async (
//     HttpContext http,
//     [FromBody] UpdateUserDto dto,
//     IUserRepository repo,
//     IJwtTokenService jwt,
//     IMapper mapper
// ) =>
// {
//         
//     var userIdStr = http.User.Claims
//         .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
//     if (userIdStr == null || !Guid.TryParse(userIdStr, out var userId))
//         return Results.Unauthorized();
//
//     var user = await repo.GetByIdAsync(userId);
//     if (user == null)
//         return Results.NotFound();
//
//     if (dto.Login != user.Login && await repo.GetByUsernameOrEmailAsync(dto.Login) is not null)
//         return Results.BadRequest("Login already taken");
//     if (dto.Email != user.Email && await repo.GetAllAsync() is var all && all.Any(u => u.Email == dto.Email))
//         return Results.BadRequest("Email already taken");
//
//     user.Login = dto.Login;
//     user.Email = dto.Email;
//
//     if (!string.IsNullOrWhiteSpace(dto.Password))
//         user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
//
//     if (!string.IsNullOrWhiteSpace(dto.AvatarBase64))
//     {
//         var ext = ".png";
//         var fileName = $"{user.Id}{ext}";
//         var filePath = Path.Combine("wwwroot/avatars", fileName);
//         var base64Data = dto.AvatarBase64.Substring(dto.AvatarBase64.IndexOf(",") + 1);
//         var bytes = Convert.FromBase64String(base64Data);
//         await File.WriteAllBytesAsync(filePath, bytes);
//         user.AvatarFileName = fileName;
//     }
//
//     await repo.UpdateAsync(user);
//
//     var updatedDto = mapper.Map<UserDto>(user);
//     var token = jwt.GenerateToken(updatedDto);
//
//     return Results.Ok(new { token, user = updatedDto });
// });
//
//
// // ADMIN
//
// // GET ALL USERS
// app.MapGet("/api/admin/users", async (
//     [AsParameters] GetUsersQuery query, 
//     IMediator mediator,
//     HttpContext httpContext
// ) =>
// {
//     var isAdmin = httpContext.User.Claims.Any(c => c.Type == "isAdmin" && c.Value == "True");
//     if (!isAdmin)
//         return Results.Forbid();
//
//     var users = await mediator.Send(query);
//     return Results.Ok(users);
// }) .RequireAuthorization();
//
//
// // GET USER BY USERNAME
//
// app.MapGet("/api/admin/user/{username}", async (
//     string username,
//     IMediator mediator,
//     HttpContext httpContext
// ) =>
// {
//     var isAdmin = httpContext.User.Claims.Any(c => c.Type == "isAdmin" && c.Value == "True");
//     if (!isAdmin)
//         return Results.Forbid();
//
//     var userDetails = await mediator.Send(new GetUserDetailsQuery(username));
//     if (userDetails == null)
//         return Results.NotFound();
//
//     return Results.Ok(userDetails);
// }).RequireAuthorization();
//
// // BlockUser/Unblock
// app.MapPost("/api/admin/user/block", async (BlockUserCommand cmd, IMediator mediator, HttpContext ctx) =>
// {
//     var isAdmin = ctx.User.Claims.Any(c => c.Type == "isAdmin" && c.Value == "True");
//     if (!isAdmin)
//         return Results.Forbid();
//
//     var adminEmail = ctx.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
//     var result = await mediator.Send(cmd with { AdminEmail = adminEmail });
//     return result ? Results.Ok() : Results.NotFound();
// }).RequireAuthorization();
//
// // Delete
// app.MapDelete("/api/admin/user/{userId}", async (Guid userId, IMediator mediator, HttpContext ctx) =>
// {
//     var isAdmin = ctx.User.Claims.Any(c => c.Type == "isAdmin" && c.Value == "True");
//     if (!isAdmin)
//         return Results.Forbid();
//
//     var result = await mediator.Send(new DeleteUserCommand(userId));
//     return result ? Results.Ok() : Results.NotFound();
// }).RequireAuthorization();
//
// // Get all snippets
//
// app.MapGet("/api/admin/snippets", async (
//     [FromQuery] bool? isPublic,
//     IMediator mediator,
//     HttpContext httpContext
// ) =>
// {
//     var isAdmin = httpContext.User.Claims.Any(c => c.Type == "isAdmin" && c.Value == "True");
//     if (!isAdmin)
//         return Results.Forbid();
//
//     var result = await mediator.Send(new GetAllSnippetsQuery(isPublic));
//     return Results.Ok(result);
// }).RequireAuthorization();
//
//
// //Get all sessions
//
// app.MapGet("/api/admin/sessions", async (
//     [FromQuery] string? search,
//     [FromQuery] bool? isActive,
//     [FromQuery] DateTime? createdFrom,
//     [FromQuery] DateTime? createdTo,
//     [FromQuery] int? minParticipants,
//     IMediator mediator,
//     HttpContext httpContext
// ) =>
// {
//     var isAdmin = httpContext.User.Claims.Any(c => c.Type == "isAdmin" && c.Value == "True");
//     if (!isAdmin) return Results.Forbid();
//
//     var sessions = await mediator.Send(new GetAllSessionsQuery(search, isActive, createdFrom, createdTo, minParticipants));
//     return Results.Ok(sessions);
// }).RequireAuthorization();
//
// // Find snippet by title
// app.MapGet("/api/admin/snippet", async (
//     [FromQuery] string title,
//     IMediator mediator,
//     HttpContext httpContext
// ) => {
//     var snippet = await mediator.Send(new GetSnippetByTitleQuery(title));
//     if (snippet == null)
//         return Results.NotFound();
//     return Results.Ok(snippet);
// }).RequireAuthorization();
//
// // Snippet Update
// app.MapPut("/api/admin/snippet", async (
//     SnippetDto snippet,
//     IMediator mediator,
//     HttpContext httpContext
// ) => {
//     var result = await mediator.Send(new UpdateSnippetCommand(snippet));
//     if (!result)
//         return Results.BadRequest();
//     return Results.Ok();
// }).RequireAuthorization();
//
//
// // Deleting snippet
// app.MapDelete("/api/admin/snippet/{id}", async (
//     Guid id,
//     IMediator mediator,
//     HttpContext httpContext
// ) =>
// {
//     var isAdmin = httpContext.User.Claims.Any(c => c.Type == "isAdmin" && c.Value == "True");
//     if (!isAdmin) return Results.Forbid();
//
//     var deleted = await mediator.Send(new DeleteSnippetCommand(id));
//     if (!deleted) return Results.NotFound();
//
//     return Results.Ok();
// }).RequireAuthorization();
//
// // GET session by id (for edit)
// app.MapGet("/api/admin/session", async (
//     [FromQuery] string name,
//     IMediator mediator,
//     HttpContext httpContext
// ) =>
// {
//     var isAdmin = httpContext.User.Claims.Any(c => c.Type == "isAdmin" && c.Value == "True");
//     if (!isAdmin) return Results.Forbid();
//
//     var session = await mediator.Send(new GetCollabSessionDetailsQuery(name));
//     if (session == null) return Results.NotFound();
//
//     return Results.Ok(session);
// }).RequireAuthorization();
//
// // PUT update session
// app.MapPut("/api/admin/session", async (
//     CollabSessionEditDto dto, IMediator mediator, HttpContext ctx
// ) => {
//     if (!ctx.User.Claims.Any(c => c.Type == "isAdmin" && c.Value == "True")) return Results.Forbid();
//     var updated = await mediator.Send(new UpdateCollabSessionCommand(dto));
//     return updated ? Results.Ok() : Results.BadRequest();
// }).RequireAuthorization();
//
// // DELETE session
// app.MapDelete("/api/admin/session/{name}", async (
//     string name,
//     IMediator mediator,
//     HttpContext httpContext
// ) =>
// {
//     var isAdmin = httpContext.User.Claims.Any(c => c.Type == "isAdmin" && c.Value == "True");
//     if (!isAdmin) return Results.Forbid();
//
//     var deleted = await mediator.Send(new DeleteCollabSessionCommand(name));
//     if (!deleted) return Results.NotFound();
//
//     return Results.Ok();
// }).RequireAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

