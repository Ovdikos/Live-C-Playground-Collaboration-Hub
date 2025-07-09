namespace Core.Entities;

public class CollabParticipant
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public Guid UserId { get; set; }

    public DateTime JoinedAt { get; set; }
    
    // public string? Role { get; set; }

    public CollabSession? Session { get; set; }
    public User? User { get; set; }
}