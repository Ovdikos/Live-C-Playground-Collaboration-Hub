using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.UnitTests.AdminTests;

public class BlockUserTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Block_And_Unblock_User()
    {
        using var context = GetDbContext();
        var user = new User { Id = Guid.NewGuid(), Username = "blockme", Email = "block@mail.com", PasswordHash = "hash" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repo = new AdminRepository(context);

        var blocked = await repo.SetUserBlockedAsync(user.Id, true, "admin@mail.com");
        var updated = await context.Users.FindAsync(user.Id);

        blocked.Should().BeTrue();
        updated.IsBlocked.Should().BeTrue();
        updated.BlockedByAdminEmail.Should().Be("admin@mail.com");

        var unblocked = await repo.SetUserBlockedAsync(user.Id, false, null);
        updated = await context.Users.FindAsync(user.Id);

        unblocked.Should().BeTrue();
        updated.IsBlocked.Should().BeFalse();
        updated.BlockedByAdminEmail.Should().BeNull();
    }

    [Fact]
    public async Task Should_Throw_If_User_Not_Found()
    {
        using var context = GetDbContext();
        var repo = new AdminRepository(context);
        Func<Task> act = async () => await repo.SetUserBlockedAsync(Guid.NewGuid(), true, "admin@mail.com");
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}