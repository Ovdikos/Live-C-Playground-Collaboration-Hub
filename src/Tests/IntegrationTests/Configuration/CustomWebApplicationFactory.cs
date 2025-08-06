using Infrastructure.DbContext;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.IntegrationTests.Configuration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var baseDir = AppContext.BaseDirectory;
        var webProject = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "Web"));
        Console.WriteLine($"BaseDir = {baseDir}");
        Console.WriteLine($"Computed WebRoot = {webProject}");
        builder
            .UseContentRoot(webProject)
            .UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.Single(
                d => d.ServiceType == typeof(DbContextOptions<LivePlaygroundDbContext>));
            services.Remove(descriptor);
            services.AddDbContext<LivePlaygroundDbContext>(opts =>
                opts.UseInMemoryDatabase("TestDb"));

            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "Test", options => { });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LivePlaygroundDbContext>();
            db.Database.EnsureCreated();
            db.Users.AddRange(
                new Core.Entities.User
                {
                    Id           = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Username     = "admin",
                    Email        = "admin@example.com",
                    PasswordHash = "irrelevant",
                    CreatedAt    = DateTime.UtcNow,
                    IsAdmin      = true
                },
                new Core.Entities.User
                {
                    Id           = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Username     = "user1",
                    Email        = "user1@example.com",
                    PasswordHash = "irrelevant",
                    CreatedAt    = DateTime.UtcNow,
                    IsAdmin      = false
                }
            );
            db.SaveChanges();
        });
    }
}