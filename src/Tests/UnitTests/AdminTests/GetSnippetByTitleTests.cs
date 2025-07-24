using Core.Entities;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.UnitTests.AdminTests;

public class GetSnippetByTitleTests
{
    private LivePlaygroundDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LivePlaygroundDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LivePlaygroundDbContext(options);
    }

    [Fact]
    public async Task Should_Return_Snippet_By_Title()
    {
        using var context = GetDbContext();
        var owner = new User
        {
            Id = Guid.NewGuid(),
            Username = "testowner",
            Email = "owner@mail.com",
            PasswordHash = "hash"
        };
        context.Users.Add(owner);

        var snippet = new CodeSnippet
        {
            Id = Guid.NewGuid(),
            Title = "myTitle",
            Content = "abc",
            OwnerId = owner.Id,
            CreatedAt = DateTime.UtcNow
        };
        context.CodeSnippets.Add(snippet);
        await context.SaveChangesAsync();

        var repo = new AdminRepository(context);
        var result = await repo.GetSnippetByTitleAsync("myTitle");
        result.Should().NotBeNull();
        result.Title.Should().Be("myTitle");
    }


    [Fact]
    public async Task Should_Return_Null_If_Not_Found()
    {
        using var context = GetDbContext();
        var repo = new AdminRepository(context);
        var result = await repo.GetSnippetByTitleAsync("notfound");
        result.Should().BeNull();
    }
}