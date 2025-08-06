using System.Net.Http.Json;
using Application.DTOs.UserDtos;
using Tests.IntegrationTests.Configuration;

namespace Tests.IntegrationTests.AdminTests;

public class GetUsersTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public GetUsersTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUsers_FilterIsAdmin_ReturnsOnlyAdmins()
    {
        var resp = await _client.GetAsync("/api/admin/users?isAdmin=true");

        resp.EnsureSuccessStatusCode();
        var users = await resp.Content.ReadFromJsonAsync<List<UserDto>>();
        Assert.Single(users);
        Assert.All(users, u => Assert.True(u.IsAdmin));
        Assert.Contains(users!, u => u.Username == "admin");
    }

    [Fact]
    public async Task GetUsers_FilterNonAdmin_ReturnsOnlyNonAdmins()
    {
        var resp = await _client.GetAsync("/api/admin/users?isAdmin=false");

        resp.EnsureSuccessStatusCode();
        var users = await resp.Content.ReadFromJsonAsync<List<UserDto>>();
        Assert.Single(users);
        Assert.All(users, u => Assert.False(u.IsAdmin));
        Assert.Contains(users!, u => u.Username == "user1");
    }

    [Fact]
    public async Task GetUsers_SearchByUsername_ReturnsMatchingUsers()
    {
        var resp = await _client.GetAsync("/api/admin/users?search=adm");

        resp.EnsureSuccessStatusCode();
        var users = await resp.Content.ReadFromJsonAsync<List<UserDto>>();
        Assert.Single(users);
        Assert.Equal("admin", users![0].Username);
    }

    [Fact]
    public async Task GetUsers_OrderByUsernameDesc_ReturnsUsersInReverseAlphabeticalOrder()
    {
        var resp = await _client.GetAsync("/api/admin/users?orderBy=username&desc=true");

        resp.EnsureSuccessStatusCode();
        var users = await resp.Content.ReadFromJsonAsync<List<UserDto>>();
        Assert.Equal(2, users!.Count);
        Assert.Equal("user1", users[0].Username);
        Assert.Equal("admin", users[1].Username);
    }
    
    [Fact]
    public async Task GetUsers_Returns_AllUsers()
    {
        var resp = await _client.GetAsync("/api/admin/users");

        resp.EnsureSuccessStatusCode();
        var users = await resp.Content.ReadFromJsonAsync<List<UserDto>>();
        Assert.Contains(users!, u => u.Username == "admin");
    }
    
}