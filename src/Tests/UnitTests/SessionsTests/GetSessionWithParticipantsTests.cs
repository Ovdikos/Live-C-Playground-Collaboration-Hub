using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.SessionsTests;

public class GetSessionWithParticipantsTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
public async Task Should_Return_Session_With_Participants()
{
    var ownerId = Guid.NewGuid();
    var participantUserId = Guid.NewGuid();
    var sessionId = Guid.NewGuid();

    using (var context = GetDbContext())
    {
        var owner = new User
        {
            Id = ownerId,
            Username = "owner",
            Email = "owner@mail.com",
            PasswordHash = "hash"
        };
        var participantUser = new User
        {
            Id = participantUserId,
            Username = "participant",
            Email = "part@mail.com",
            PasswordHash = "hash"
        };
        context.Users.AddRange(owner, participantUser);

        var snippet = new CodeSnippet
        {
            Id = Guid.NewGuid(),
            Title = "Snippet1",
            Content = "some code",
            OwnerId = ownerId,
            Owner = owner
        };
        context.CodeSnippets.Add(snippet);

        var session = new CollabSession
        {
            Id = sessionId,
            Name = "Session1",
            OwnerId = ownerId,
            Owner = owner,
            CodeSnippetId = snippet.Id,
            CodeSnippet = snippet,
            CreatedAt = DateTime.UtcNow
        };
        context.CollabSessions.Add(session);

        var participant = new CollabParticipant
        {
            Id = Guid.NewGuid(),
            SessionId = sessionId,
            Session = session,
            UserId = participantUserId,
            User = participantUser,
            JoinedAt = DateTime.UtcNow
        };
        context.CollabParticipants.Add(participant);

        await context.SaveChangesAsync();

        var repo = new CollabParticipantRepository(context);

        var result = await repo.GetSessionWithParticipantsAsync(sessionId);

        result.Should().NotBeNull();
        result.Participants.Should().HaveCount(1);
        result.Participants.First().User.Username.Should().Be("participant");
    }
}

    [Fact]
    public async Task Should_Return_Null_If_Session_Not_Exists()
    {
        var context = GetDbContext();
        var repo = new CollabParticipantRepository(context);

        var result = await repo.GetSessionWithParticipantsAsync(Guid.NewGuid());

        result.Should().BeNull();
    }
}
