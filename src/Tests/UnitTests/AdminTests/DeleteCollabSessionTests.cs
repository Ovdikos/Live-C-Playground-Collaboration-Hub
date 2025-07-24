using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.UnitTests.AdminTests;

public class DeleteCollabSessionTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Delete_Session_By_Name()
    {
        using var context = GetDbContext();
        var session = new CollabSession
        {
            Id = Guid.NewGuid(),
            Name = "del-session",
            OwnerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };
        context.CollabSessions.Add(session);
        await context.SaveChangesAsync();

        var repo = new AdminRepository(context);
        var deleted = await repo.DeleteSessionByNameAsync("del-session");
        deleted.Should().BeTrue();
        context.CollabSessions.Count().Should().Be(0);
    }

    [Fact]
    public async Task Should_Return_False_If_Session_Not_Found()
    {
        using var context = GetDbContext();
        var repo = new AdminRepository(context);
        var deleted = await repo.DeleteSessionByNameAsync("notfound");
        deleted.Should().BeFalse();
    }
}