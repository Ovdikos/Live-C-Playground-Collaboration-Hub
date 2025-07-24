using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.UnitTests.AdminTests;

public class DeleteUserTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Delete_User()
    {
        using var context = GetDbContext();
        var user = new User { Id = Guid.NewGuid(), Username = "delme", Email = "d@mail.com", PasswordHash = "hash" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repo = new AdminRepository(context);
        var deleted = await repo.DeleteUserAsync(user.Id);

        deleted.Should().BeTrue();
        context.Users.Count().Should().Be(0);
    }

    [Fact]
    public async Task Should_Return_False_If_User_Not_Found()
    {
        using var context = GetDbContext();
        var repo = new AdminRepository(context);
        var deleted = await repo.DeleteUserAsync(Guid.NewGuid());
        deleted.Should().BeFalse();
    }
}