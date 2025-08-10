using System.Net;
using System.Net.Http.Json;
using Application.DTOs.SessionDtos;
using Infrastructure.DbContext;
using Microsoft.Extensions.DependencyInjection;
using Tests.IntegrationTests.Configuration;

namespace Tests.IntegrationTests.AdminTests;

public class GetCollabSessionDetailsTests : IClassFixture<CustomWebApplicationFactory>
{
    
    private readonly HttpClient _client;
    private readonly IServiceScopeFactory _scopeFactory;

    public GetCollabSessionDetailsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
    }


    [Fact]
    public async Task GetSessionByName_Existing_ReturnsDetails()
    {
        string sessionName = "Test Session A";
        string snippetTitle = "Snippet A";
        string snippetContent = "Console.WriteLine(\"Hello\");";

        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<LivePlaygroundDbContext>();

            var owner  = db.Users.Single(u => u.Username == "user1");
            var admin  = db.Users.Single(u => u.Username == "admin");

            var snippet = new Core.Entities.CodeSnippet
            {
                Id = Guid.NewGuid(),
                Title = snippetTitle,
                Content = snippetContent,
                OwnerId = owner.Id,
                CreatedAt = DateTime.UtcNow,
                IsPublic = true
            };
            db.CodeSnippets.Add(snippet);

            var session = new Core.Entities.CollabSession
            {
                Id = Guid.NewGuid(),
                Name = sessionName,
                OwnerId = owner.Id,
                CodeSnippetId = snippet.Id,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                JoinCode = "JOIN123"
            };
            db.CollabSessions.Add(session);

            db.CollabParticipants.AddRange(
                new Core.Entities.CollabParticipant
                {
                    Id = Guid.NewGuid(),
                    SessionId = session.Id,
                    UserId = owner.Id,
                    JoinedAt = DateTime.UtcNow
                },
                new Core.Entities.CollabParticipant
                {
                    Id = Guid.NewGuid(),
                    SessionId = session.Id,
                    UserId = admin.Id,
                    JoinedAt = DateTime.UtcNow
                }
            );

            db.SessionEditHistories.Add(
                new Core.Entities.SessionEditHistory
                {
                    Id = Guid.NewGuid(),
                    SessionId = session.Id,
                    EditedByUserId = admin.Id,
                    EditedAt = DateTime.UtcNow,
                    Changes = "Initial content"
                }
            );

            db.SaveChanges();
        }

        var resp = await _client.GetAsync($"/api/admin/session?name={Uri.EscapeDataString(sessionName)}");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var dto = await resp.Content.ReadFromJsonAsync<CollabSessionDetailsDto>();
        Assert.NotNull(dto);
        Assert.Equal(sessionName, dto!.Name);
        Assert.Equal("user1", dto.OwnerName);
        Assert.Equal(snippetTitle, dto.CodeSnippetTitle);
        Assert.Equal(snippetContent, dto.CodeSnippetContent);
        Assert.True(dto.IsActive);
        Assert.Equal("JOIN123", dto.JoinCode);

        Assert.True(dto.Participants.Count >= 2);
        var participantNames = dto.Participants.Select(p => p.Username).ToList();
        Assert.Contains("user1", participantNames);
        Assert.Contains("admin", participantNames);

        Assert.True(dto.EditHistories.Count >= 1);
    }

    [Fact]
    public async Task GetSessionByName_NotFound_Returns404()
    {
        var resp = await _client.GetAsync("/api/admin/session?name=__no_such_session__");
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }
    
}