using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.UnitTests.AdminTests;

public class DeleteSnippetTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Delete_Snippet()
    {
        using var context = GetDbContext();
        var snippet = new CodeSnippet
        {
            Id = Guid.NewGuid(),
            Title = "del",
            Content = "del",
            OwnerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };
        context.CodeSnippets.Add(snippet);
        await context.SaveChangesAsync();

        var repo = new AdminRepository(context);
        var deleted = await repo.DeleteSnippetAsync(snippet.Id);
        deleted.Should().BeTrue();
        context.CodeSnippets.Count().Should().Be(0);
    }

    [Fact]
    public async Task Should_Return_False_If_Snippet_Not_Found()
    {
        using var context = GetDbContext();
        var repo = new AdminRepository(context);
        var deleted = await repo.DeleteSnippetAsync(Guid.NewGuid());
        deleted.Should().BeFalse();
    }
}