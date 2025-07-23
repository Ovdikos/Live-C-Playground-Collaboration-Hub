using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.SnippetsTests;

public class GetCodeSnippetByIdTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Return_Snippet_When_Exists()
    {
        var context = GetDbContext();
        var repo = new CodeSnippetRepository(context);

        var owner = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@email.com",
            PasswordHash = "hash"
        };
        await context.Users.AddAsync(owner);

        var snippet = new CodeSnippet
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            Content = "some code here",
            OwnerId = owner.Id, 
            CreatedAt = DateTime.UtcNow
        };
        await context.CodeSnippets.AddAsync(snippet);
        await context.SaveChangesAsync();

        var result = await repo.GetByIdAsync(snippet.Id);

        result.Should().NotBeNull();
        result.Title.Should().Be(snippet.Title);
        result.Owner.Should().NotBeNull();
        result.Owner.Username.Should().Be(owner.Username);
    }


    [Fact]
    public async Task Should_Return_Null_When_NotFound()
    {
        var context = GetDbContext();
        var repo = new CodeSnippetRepository(context);

        var result = await repo.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }
}