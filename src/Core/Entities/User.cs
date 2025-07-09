namespace Core.Entities;

public class User
{

    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? AvatarUrl { get; set; }
    public bool IsAdmin { get; set; }
    
    
    public ICollection<CodeSnippet> CodeSnippets { get; set; } = new List<CodeSnippet>();
    
    // For future (maybe)
    public ICollection<CollabParticipant> Sessions { get; set; } = new List<CollabParticipant>();
    
    
}