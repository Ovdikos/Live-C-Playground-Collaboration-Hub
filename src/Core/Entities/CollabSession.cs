namespace Core.Entities;

public class CollabSession
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty; 
    public Guid OwnerId { get; set; }                
    public Guid CodeSnippetId { get; set; }          
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }         
    public DateTime? EditedAt { get; set; }         
    public bool IsActive { get; set; } = true;
    
    public string JoinCode { get; set; } = string.Empty;

    
    public User? Owner { get; set; }
    public CodeSnippet? CodeSnippet { get; set; }
    public ICollection<CollabParticipant> Participants { get; set; } = new List<CollabParticipant>();
    
    public ICollection<SessionEditHistory> EditHistories { get; set; } = new List<SessionEditHistory>();
}