namespace Application.DTOs;

public class CollabSessionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid CodeSnippetId { get; set; }
    public string? CodeSnippetTitle { get; set; }
    public Guid OwnerId { get; set; }
    public List<CollabParticipantDto> Participants { get; set; } = new();
}