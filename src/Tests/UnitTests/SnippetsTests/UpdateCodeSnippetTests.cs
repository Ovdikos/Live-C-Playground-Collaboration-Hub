using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.SnippetsTests;

public class UpdateCodeSnippetTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Update_Snippet_When_Exists()
    {
        var context = GetDbContext();
        var repo = new CodeSnippetRepository(context);
        var snippet = new CodeSnippet
        {
            Id = Guid.NewGuid(),
            Title = "Old Title",
            Content = "Old",
            OwnerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };
        await context.CodeSnippets.AddAsync(snippet);
        await context.SaveChangesAsync();

        snippet.Title = "New Title";
        snippet.Content = "Updated content";
        await repo.UpdateAsync(snippet);

        var dbSnippet = await context.CodeSnippets.FirstOrDefaultAsync(s => s.Id == snippet.Id);
        dbSnippet.Title.Should().Be("New Title");
        dbSnippet.Content.Should().Be("Updated content");
    }
}