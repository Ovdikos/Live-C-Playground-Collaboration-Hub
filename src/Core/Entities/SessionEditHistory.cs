namespace Core.Entities;

public class SessionEditHistory
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public Guid EditedByUserId { get; set; }
    public DateTime? EditedAt { get; set; }
    public string Changes { get; set; } = null!;

    public CollabSession? Session { get; set; }
    public User? EditedByUser { get; set; }
}