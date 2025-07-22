namespace Application.DTOs.SessionDtos;

public class CollabSessionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Content { get; set; }
    public string? OwnerName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CodeSnippetTitle { get; set; }
    public List<CollabParticipantDto> Participants { get; set; } = new();
    
    public List<SessionEditHistoryDto> EditHistories { get; set; } = new();
    
    public string? SnippetContent { get; set; }
    
    public DateTime ExpiresAt { get; set; }

}    