namespace Core.Entities;

public class User
{

    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? AvatarFileName { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsBlocked { get; set; } = false; // for admin
    
    
    public ICollection<CodeSnippet> CodeSnippets { get; set; } = new List<CodeSnippet>();
    
    public ICollection<CollabSession> OwnedSessions { get; set; } = new List<CollabSession>();
    // For future (maybe)
    public ICollection<CollabParticipant> CollabParticipants { get; set; } = new List<CollabParticipant>();
    
    
}