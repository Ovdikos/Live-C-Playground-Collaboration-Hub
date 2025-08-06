using System.Net;
using System.Net.Http.Json;
using Application.DTOs.UserDtos;
using Tests.IntegrationTests.Configuration;

namespace Tests.IntegrationTests.AdminTests;

public class UserDetailsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UserDetailsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUserByUsername_ExistingUser_ReturnsUserDetails()
    {
        var existingUsername = "user1";

        var response = await _client.GetAsync($"/api/admin/user/{existingUsername}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var dto = await response.Content.ReadFromJsonAsync<UserDetailsDto>();
        Assert.NotNull(dto);
        Assert.Equal(existingUsername, dto!.Username);
        Assert.Equal("user1@example.com", dto.Email);
        Assert.False(dto.IsAdmin);
        Assert.False(dto.IsBlocked);
        Assert.Empty(dto.CodeSnippets);
        Assert.Empty(dto.OwnedSessions);
        Assert.Empty(dto.CollabSessions);
    }

    [Fact]
    public async Task GetUserByUsername_Nonexistent_ReturnsNotFound()
    {
        var nonUser = "this_user_does_not_exist";

        var response = await _client.GetAsync($"/api/admin/user/{nonUser}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}