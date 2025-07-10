namespace Application.DTOs;

public class CollabSessionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? OwnerName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CodeSnippetTitle { get; set; }
    public List<CollabParticipantDto> Participants { get; set; } = new();
}