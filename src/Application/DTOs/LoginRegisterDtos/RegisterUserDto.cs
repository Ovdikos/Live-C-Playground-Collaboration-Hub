using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.LoginRegisterDtos;

public class RegisterUserDto
{
    [Required]
    [StringLength(32, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username must be alphanumeric (latin letters, numbers, underscore).")]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [StringLength(128, MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$",
        ErrorMessage = "Password must contain at least 8 characters, one uppercase, one lowercase, one number, and one special character.")]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    
    public string? AvatarUrl { get; set; }
}