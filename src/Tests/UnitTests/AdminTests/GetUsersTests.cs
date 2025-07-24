using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.UnitTests.AdminTests;

public class GetUsersTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Return_All_Users()
    {
        using var context = GetDbContext();
        context.Users.AddRange(
            new User { Id = Guid.NewGuid(), Username = "admin", Email = "a@mail.com", PasswordHash = "hash", IsAdmin = true },
            new User { Id = Guid.NewGuid(), Username = "user", Email = "u@mail.com", PasswordHash = "hash" }
        );
        await context.SaveChangesAsync();

        var repo = new AdminRepository(context);
        var result = await repo.GetUsersAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Select(x => x.Username).Should().Contain(new[] { "admin", "user" });
    }

    [Fact]
    public async Task Should_Return_Users_With_Search_And_Filter()
    {
        using var context = GetDbContext();
        context.Users.AddRange(
            new User { Id = Guid.NewGuid(), Username = "admin1", Email = "adm1@mail.com", PasswordHash = "hash", IsAdmin = true },
            new User { Id = Guid.NewGuid(), Username = "admin2", Email = "adm2@mail.com", PasswordHash = "hash", IsAdmin = true },
            new User { Id = Guid.NewGuid(), Username = "normal", Email = "n@mail.com", PasswordHash = "hash", IsAdmin = false }
        );
        await context.SaveChangesAsync();

        var repo = new AdminRepository(context);
        var result = await repo.GetUsersAsync(search: "admin", isAdmin: true);

        result.Should().HaveCount(2);
        result.All(u => u.IsAdmin).Should().BeTrue();
        result.All(u => u.Username.Contains("admin")).Should().BeTrue();
    }
}
