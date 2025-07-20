using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.Features.Admin.CleanupService;

public class SessionCleanupService : BackgroundService
{
    private readonly IServiceProvider _services;
    public SessionCleanupService(IServiceProvider services) => _services = services;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<LivePlaygroundDbContext>();
                var expired = await db.CollabSessions.Where(s => s.ExpiresAt != null && s.ExpiresAt <= DateTime.Now).ToListAsync();
                db.CollabSessions.RemoveRange(expired);
                await db.SaveChangesAsync();
            }
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
