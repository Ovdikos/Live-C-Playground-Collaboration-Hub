﻿namespace Application.DTOs.SnippetsDtos;

public class SnippetDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public Guid OwnerId { get; set; }
    public string OwnerName { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsPublic { get; set; }
}