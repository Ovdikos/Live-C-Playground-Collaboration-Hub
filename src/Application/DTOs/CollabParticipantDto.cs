namespace Application.DTOs;

public class CollabParticipantDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
}