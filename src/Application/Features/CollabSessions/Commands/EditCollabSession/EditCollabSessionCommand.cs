using Application.DTOs.SessionDtos;
using MediatR;

namespace Application.Features.CollabSessions.Commands.EditCollabSession;

public class EditCollabSessionCommand : IRequest<CollabSessionDto>
{
    public Guid SessionId { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public string Content { get; set; } = string.Empty;
    public Guid EditedByUserId { get; set; }
    public string Changes { get; set; } = string.Empty;
}
