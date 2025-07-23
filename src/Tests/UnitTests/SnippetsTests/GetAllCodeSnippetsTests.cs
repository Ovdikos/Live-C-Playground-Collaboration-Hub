using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.SnippetsTests;

public class GetAllCodeSnippetsTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Return_Snippets_For_User()
    {
        var context = GetDbContext();
        var repo = new CodeSnippetRepository(context);
        var ownerId = Guid.NewGuid();

        var snippet1 = new CodeSnippet
        {
            Id = Guid.NewGuid(),
            OwnerId = ownerId,
            Title = "One",
            Content = "some code here",
            CreatedAt = DateTime.UtcNow
        };
        var snippet2 = new CodeSnippet
        {
            Id = Guid.NewGuid(),
            OwnerId = ownerId,
            Title = "Two",
            Content = "some code here", 
            CreatedAt = DateTime.UtcNow
        };

        await context.CodeSnippets.AddRangeAsync(snippet1, snippet2);
        await context.SaveChangesAsync();

        var result = await repo.GetAllAsync(ownerId);

        result.Should().HaveCount(2);
        result.Should().OnlyContain(s => s.OwnerId == ownerId);
    }

    [Fact]
    public async Task Should_Return_EmptyList_When_UserHasNoSnippets()
    {
        var context = GetDbContext();
        var repo = new CodeSnippetRepository(context);

        var result = await repo.GetAllAsync(Guid.NewGuid());

        result.Should().BeEmpty();
    }
}