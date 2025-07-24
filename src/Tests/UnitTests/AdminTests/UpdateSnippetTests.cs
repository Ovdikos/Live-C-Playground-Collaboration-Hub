using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.UnitTests.AdminTests;

public class UpdateSnippetTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Update_Snippet()
    {
        using var context = GetDbContext();
        var snippet = new CodeSnippet
        {
            Id = Guid.NewGuid(),
            Title = "toEdit",
            Content = "x",
            OwnerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };
        context.CodeSnippets.Add(snippet);
        await context.SaveChangesAsync();

        snippet.Title = "updated";
        var repo = new AdminRepository(context);
        var updated = await repo.UpdateSnippetAsync(snippet);
        updated.Should().BeTrue();

        var refreshed = await context.CodeSnippets.FindAsync(snippet.Id);
        refreshed.Title.Should().Be("updated");
    }
}