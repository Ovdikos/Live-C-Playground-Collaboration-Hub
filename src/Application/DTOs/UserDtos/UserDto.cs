﻿namespace Application.DTOs.UserDtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AvatarFileName { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsAdmin { get; set; }
}