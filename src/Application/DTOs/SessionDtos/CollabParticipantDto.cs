namespace Application.DTOs.SessionDtos;

public class CollabParticipantDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public DateTime JoinedAt { get; set; }
}