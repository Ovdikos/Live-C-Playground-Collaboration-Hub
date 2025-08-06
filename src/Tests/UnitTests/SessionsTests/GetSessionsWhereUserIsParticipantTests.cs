using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.SessionsTests;

public class GetSessionsWhereUserIsParticipantTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Return_Sessions_Where_User_Is_Participant()
    {
        // TODO: need to fix this peace of test :), idk why, it's stop working
        var userId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();

        using (var context = GetDbContext())
        {
            var owner = new User { Id = ownerId, Username = "owner", Email = "o@mail.com", PasswordHash = "hash" };
            var participantUser = new User { Id = userId, Username = "participant", Email = "p@mail.com", PasswordHash = "hash" };
            context.Users.AddRange(owner, participantUser);

            var snippet = new CodeSnippet
            {
                Id = Guid.NewGuid(),
                Title = "Test Snippet",
                Content = "code",
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
                UserId = userId,
                User = participantUser,
                JoinedAt = DateTime.UtcNow
            };
            context.CollabParticipants.Add(participant);

            await context.SaveChangesAsync();
        }

        using (var context = GetDbContext())
        {
            var repo = new CollabParticipantRepository(context);
            var result = await repo.GetSessionsWhereUserIsParticipantAsync(userId);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Id.Should().Be(sessionId);
            result.First().Name.Should().Be("Session1");
        }
    }

    [Fact]
    public async Task Should_Return_Empty_List_When_User_Not_Participant()
    {
        var context = GetDbContext();
        var repo = new CollabParticipantRepository(context);

        var result = await repo.GetSessionsWhereUserIsParticipantAsync(Guid.NewGuid());

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
