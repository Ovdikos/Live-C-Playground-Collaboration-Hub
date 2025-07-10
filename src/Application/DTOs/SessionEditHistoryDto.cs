namespace Application.DTOs;

public class SessionEditHistoryDto
{
    public DateTime EditedAt { get; set; }
    public string EditedByUsername { get; set; } = string.Empty;
    public string Changes { get; set; } = string.Empty;
}