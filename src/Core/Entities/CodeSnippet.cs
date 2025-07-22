namespace Core.Entities;

public class CodeSnippet
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public Guid OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsPublic { get; set; }
    
    public User? Owner { get; set; }
    
    public ICollection<CollabSession> CollabSessions { get; set; } = new List<CollabSession>();
    
}