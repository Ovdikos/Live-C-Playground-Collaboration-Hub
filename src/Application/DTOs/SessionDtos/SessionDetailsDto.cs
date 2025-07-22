namespace Application.DTOs.SessionDtos;

public class SessionDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string JoinCode { get; set; } = "";
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public string? Owner { get; set; }
    public string? CodeSnippetTitle { get; set; }
}