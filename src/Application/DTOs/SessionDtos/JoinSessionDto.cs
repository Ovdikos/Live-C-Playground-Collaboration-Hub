namespace Application.DTOs.SessionDtos;

public class JoinSessionDto
{
    public string JoinCode { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}