using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Tests.UnitTests.SessionsTests;

public class GetSessionsCreatedByUserTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public GetSessionsCreatedByUserTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Return_Sessions_Created_By_User()
    {
        var ownerId = Guid.NewGuid();

        var context = GetDbContext();

        var owner = new User
        {
            Id = ownerId,
            Username = "owner",
            Email = "owner@mail.com",
            PasswordHash = "hash"
        };
        context.Users.Add(owner);

        var codeSnippet = new CodeSnippet
        {
            Id = Guid.NewGuid(),
            Title = "Test Snippet",
            Content = "console.log('Hello');",
            OwnerId = ownerId,
            Owner = owner
        };
        context.CodeSnippets.Add(codeSnippet);

        var session = new CollabSession
        {
            Id = Guid.NewGuid(),
            Name = "Session1",
            OwnerId = ownerId,
            Owner = owner,
            CodeSnippetId = codeSnippet.Id,
            CodeSnippet = codeSnippet,
            CreatedAt = DateTime.UtcNow
        };
        context.CollabSessions.Add(session);

        await context.SaveChangesAsync();

        var repo = new CollabParticipantRepository(context);
        var result = await repo.GetSessionsCreatedByUserAsync(ownerId);

        _testOutputHelper.WriteLine($"[RESULT] Sessions found: {result.Count}");

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Session1");
        result.First().OwnerId.Should().Be(ownerId);
    }


    

    [Fact]
    public async Task Should_Return_Empty_List_When_User_Has_No_Sessions()
    {
        var context = GetDbContext();
        var repo = new CollabParticipantRepository(context);
    
        var result = await repo.GetSessionsCreatedByUserAsync(Guid.NewGuid());
    
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}