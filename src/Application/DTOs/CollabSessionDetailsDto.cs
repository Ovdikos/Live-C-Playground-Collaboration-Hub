namespace Application.DTOs;

public class CollabSessionDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string CodeSnippetTitle { get; set; } = "";
    public string? CodeSnippetContent { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public bool IsActive { get; set; }
    public string JoinCode { get; set; } = "";
    public string OwnerName { get; set; } = "";
    public List<CollabParticipantDto> Participants { get; set; } = new();
    public List<SessionEditHistoryDto> EditHistories { get; set; } = new();
}