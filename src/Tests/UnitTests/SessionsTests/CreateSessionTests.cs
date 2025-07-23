using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.SessionsTests;

public class CreateSessionTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Create_Session()
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
        await context.SaveChangesAsync();

        var session = new CollabSession
        {
            Id = Guid.NewGuid(),
            Name = "TestSession",
            OwnerId = owner.Id,
            Owner = owner
        };

        var repo = new CollabParticipantRepository(context);
        var result = await repo.CreateSessionAsync(session);

        result.Should().NotBeNull();
        result.Id.Should().Be(session.Id);

        var sessionInDb = await context.CollabSessions.FindAsync(session.Id);
        sessionInDb.Should().NotBeNull();
        sessionInDb.Name.Should().Be("TestSession");
    }
}
