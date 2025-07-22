using Application.DTOs.SessionDtos;
using Application.DTOs.SnippetsDtos;

namespace Application.DTOs.UserDtos;

public class UserDetailsDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsAdmin { get; set; }
    
    public string PasswordHash { get; set; } = null!;
    
    public bool IsBlocked { get; set; } = false; 
    public string? BlockedByAdminEmail { get; set; }
    public List<SnippetDetailsDto> CodeSnippets { get; set; } = new();
    public List<SessionDetailsDto> OwnedSessions { get; set; } = new();
    public List<SessionDetailsDto> CollabSessions { get; set; } = new();
}