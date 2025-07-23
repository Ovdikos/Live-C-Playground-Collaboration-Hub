using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.SnippetsTests;

public class DeleteCodeSnippetTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Delete_Snippet_When_Exists()
    {
        var context = GetDbContext();
        var repo = new CodeSnippetRepository(context);
        var snippet = new CodeSnippet
        {
            Id = Guid.NewGuid(),
            Title = "ToDelete",
            Content = "some code here",
            OwnerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };
        await context.CodeSnippets.AddAsync(snippet);
        await context.SaveChangesAsync();

        await repo.DeleteAsync(snippet.Id);

        var dbSnippet = await context.CodeSnippets.FindAsync(snippet.Id);
        dbSnippet.Should().BeNull();
    }

    [Fact]
    public async Task Should_Throw_When_Snippet_NotFound()
    {
        var context = GetDbContext();
        var repo = new CodeSnippetRepository(context);
        var randomId = Guid.NewGuid();

        await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteAsync(randomId));
    }
}