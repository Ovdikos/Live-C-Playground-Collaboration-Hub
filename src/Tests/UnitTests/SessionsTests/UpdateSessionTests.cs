using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.SessionsTests;

public class UpdateSessionTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Update_Session_And_Add_EditHistory()
    {
        var context = GetDbContext();
        var owner = new User
        {
            Id = Guid.NewGuid(),
            Username = "owner",
            Email = "owner@mail.com",
            PasswordHash = "hash"
        };
        await context.Users.AddAsync(owner);

        var session = new CollabSession
        {
            Id = Guid.NewGuid(),
            Name = "TestSession",
            OwnerId = owner.Id,
            Owner = owner
        };
        await context.CollabSessions.AddAsync(session);
        await context.SaveChangesAsync();

        var history = new SessionEditHistory
        {
            Id = Guid.NewGuid(),
            SessionId = session.Id,
            EditedByUserId = owner.Id,
            EditedAt = DateTime.UtcNow,
            Changes = "Changed name"
        };

        session.Name = "NewName";

        var repo = new CollabParticipantRepository(context);
        var result = await repo.UpdateSessionAsync(session, history);

        result.Name.Should().Be("NewName");
        context.SessionEditHistories.Should().ContainSingle(h => h.SessionId == session.Id && h.Changes == "Changed name");
    }
}
