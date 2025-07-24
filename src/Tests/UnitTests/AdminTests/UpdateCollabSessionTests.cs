using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.UnitTests.AdminTests;

public class UpdateCollabSessionTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Update_Session()
    {
        using var context = GetDbContext();
        var session = new CollabSession
        {
            Id = Guid.NewGuid(),
            Name = "old",
            OwnerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };
        context.CollabSessions.Add(session);
        await context.SaveChangesAsync();

        session.Name = "updated";
        var repo = new AdminRepository(context);
        var updated = await repo.UpdateAsync(session);

        updated.Should().BeTrue();
        (await context.CollabSessions.FindAsync(session.Id)).Name.Should().Be("updated");
    }
}