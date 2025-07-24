using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.UnitTests.AdminTests;

public class GetUserDetailsTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Return_User_With_Details()
    {
        using var context = GetDbContext();
        var user = new User { Id = Guid.NewGuid(), Username = "testuser", Email = "t@mail.com", PasswordHash = "hash" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repo = new AdminRepository(context);
        var result = await repo.GetUserDetailsByUsernameAsync("testuser");

        result.Should().NotBeNull();
        result.Username.Should().Be("testuser");
    }

    [Fact]
    public async Task Should_Return_Null_When_User_Not_Exists()
    {
        using var context = GetDbContext();
        var repo = new AdminRepository(context);

        var result = await repo.GetUserDetailsByUsernameAsync("notfound");
        result.Should().BeNull();
    }
}