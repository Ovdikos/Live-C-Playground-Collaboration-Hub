using System.Net;
using System.Net.Http.Json;
using Application.DTOs.SnippetsDtos;
using Infrastructure.DbContext;
using Microsoft.Extensions.DependencyInjection;
using Tests.IntegrationTests.Configuration;

namespace Tests.IntegrationTests.AdminTests;

public class GetSnippetByTitleTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly IServiceScopeFactory _scopeFactory;

    public GetSnippetByTitleTests(CustomWebApplicationFactory f)
    {
        _client = f.CreateClient();
        _scopeFactory = f.Services.GetRequiredService<IServiceScopeFactory>();
    }

    [Fact]
    public async Task FindSnippetByTitle_Existing_ReturnsDto()
    {
        string title = "Hello World";
        Guid ownerId;

        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<LivePlaygroundDbContext>();
            var owner = db.Users.Single(u => u.Username == "user1");
            ownerId = owner.Id;

            db.CodeSnippets.Add(new Core.Entities.CodeSnippet
            {
                Id = Guid.NewGuid(),
                Title = title,
                Content = "Console.WriteLine(\"Hi\");",
                OwnerId = owner.Id,
                CreatedAt = DateTime.UtcNow,
                IsPublic = true
            });
            db.SaveChanges();
        }

        var resp = await _client.GetAsync($"/api/admin/snippet?title={Uri.EscapeDataString(title)}");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        var dto = await resp.Content.ReadFromJsonAsync<SnippetDto>();
        Assert.NotNull(dto);
        Assert.Equal(title, dto!.Title);
        Assert.Equal(ownerId, dto.OwnerId);
        Assert.Equal("user1", dto.OwnerName);
        Assert.Equal("Console.WriteLine(\"Hi\");", dto.Content);
        Assert.True(dto.IsPublic);
    }

    [Fact]
    public async Task FindSnippetByTitle_NotFound_Returns404()
    {
        var resp = await _client.GetAsync("/api/admin/snippet?title=does-not-exist");
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }
}
