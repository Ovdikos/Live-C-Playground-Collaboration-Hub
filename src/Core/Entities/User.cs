namespace Core.Entities;

public class User
{

    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string? AvatarUrl { get; set; }
    
    
    public ICollection<CodeSnippet> CodeSnippets { get; set; } = new List<CodeSnippet>();
    public ICollection<CollabParticipant> Sessions { get; set; } = new List<CollabParticipant>();
    
    
}