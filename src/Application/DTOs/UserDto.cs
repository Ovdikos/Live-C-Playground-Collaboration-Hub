namespace Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsAdmin { get; set; }
}