using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.SessionsTests;

public class GetCollabSessionByIdTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Return_Session_When_Exists()
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
            Name = "Session1",
            OwnerId = owner.Id,
            Owner = owner
        };
        await context.CollabSessions.AddAsync(session);
        await context.SaveChangesAsync();

        var repo = new CollabParticipantRepository(context);

        var result = await repo.GetByIdAsync(session.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(session.Id);
        result.Owner.Should().NotBeNull();
        result.Owner.Username.Should().Be(owner.Username);
    }

    [Fact]
    public async Task Should_Return_Null_When_Session_Not_Found()
    {
        var context = GetDbContext();
        var repo = new CollabParticipantRepository(context);

        var result = await repo.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }
}