using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContext;

public class LivePlaygroundDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public LivePlaygroundDbContext(DbContextOptions<LivePlaygroundDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<CodeSnippet> CodeSnippets => Set<CodeSnippet>();
    public DbSet<CollabSession> CollabSessions => Set<CollabSession>();
    public DbSet<CollabParticipant> CollabParticipants => Set<CollabParticipant>();
    public DbSet<SessionEditHistory> SessionEditHistories => Set<SessionEditHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.Username).IsRequired().HasMaxLength(64);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(128);
            entity.Property(e => e.IsBlocked).HasDefaultValue(false);
        });

        // CodeSnippet
        modelBuilder.Entity<CodeSnippet>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(128);
            entity.Property(e => e.Content).IsRequired();
            entity.HasOne(e => e.Owner)
                  .WithMany(u => u.CodeSnippets)
                  .HasForeignKey(e => e.OwnerId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // CollabSession
        modelBuilder.Entity<CollabSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(128);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasOne(s => s.Owner)
                  .WithMany(u => u.OwnedSessions)
                  .HasForeignKey(s => s.OwnerId)
                  .OnDelete(DeleteBehavior.Restrict); 

            entity.HasOne(s => s.CodeSnippet)
                  .WithMany(sn => sn.CollabSessions)
                  .HasForeignKey(s => s.CodeSnippetId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // CollabParticipant
        modelBuilder.Entity<CollabParticipant>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.HasOne(p => p.Session)
                .WithMany(s => s.Participants)
                .HasForeignKey(p => p.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.User)
                .WithMany(u => u.CollabParticipants)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); 
        });
        
        // SessionEditHistory
        modelBuilder.Entity<SessionEditHistory>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Session)
                .WithMany(s => s.EditHistories)
                .HasForeignKey(e => e.SessionId)
                .OnDelete(DeleteBehavior.Cascade);


            entity.HasOne(e => e.EditedByUser)
                .WithMany()
                .HasForeignKey(e => e.EditedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
