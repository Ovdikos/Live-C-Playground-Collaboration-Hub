using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.SnippetsTests;

public class CreateCodeSnippetTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Add_Snippet_When_DataIsValid()
    {
        var context = GetDbContext();
        var repo = new CodeSnippetRepository(context);
        var snippet = new CodeSnippet
        {
            Id = Guid.NewGuid(),
            Title = "Test Snippet",
            Content = "Console.WriteLine();",
            OwnerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        await repo.AddAsync(snippet);

        var dbSnippet = await context.CodeSnippets.FirstOrDefaultAsync(s => s.Id == snippet.Id);
        dbSnippet.Should().NotBeNull();
        dbSnippet.Title.Should().Be(snippet.Title);
    }
}