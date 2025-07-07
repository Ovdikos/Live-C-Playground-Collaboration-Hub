namespace Core.Entities;

public class CollabSession
{
    public Guid Id { get; set; }
    public Guid SnippetId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    
    
    public CodeSnippet Snippet { get; set; } = default!;
    public ICollection<CollabParticipant> Participants { get; set; } = new List<CollabParticipant>();
    
}