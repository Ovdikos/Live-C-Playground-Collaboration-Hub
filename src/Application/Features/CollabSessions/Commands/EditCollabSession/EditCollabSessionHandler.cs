using Application.DTOs.SessionDtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace Application.Features.CollabSessions.Commands.EditCollabSession;

public class EditCollabSessionHandler : IRequestHandler<EditCollabSessionCommand, CollabSessionDto>
{
    private readonly ICollabParticipantSessionRepository _sessionRepo;
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;

    public EditCollabSessionHandler(ICollabParticipantSessionRepository sessionRepo, IUserRepository userRepo, IMapper mapper)
    {
        _sessionRepo = sessionRepo;
        _userRepo = userRepo;
        _mapper = mapper;
    }

    public async Task<CollabSessionDto> Handle(EditCollabSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepo.GetByIdAsync(request.SessionId);
        
        if (session == null) throw new Exception("Session not found ID: {request.SessionId} " );
        
        if (!session.IsActive) throw new UnauthorizedAccessException("Session is not active");
        
        var user = await _userRepo.GetByIdAsync(request.EditedByUserId);
        if (user == null)
            throw new Exception($"Editing user not found! UserId: {request.EditedByUserId}");

        string changesDesc = "";
        bool isChanged = false;

        if (!string.IsNullOrWhiteSpace(request.Changes))
            changesDesc += request.Changes.Trim() + " ";

        if (!string.IsNullOrWhiteSpace(request.Name) && session.Name != request.Name)
        {
            session.Name = request.Name;
            isChanged = true;
        }

        if (!string.IsNullOrWhiteSpace(request.Content))
        {
            if (session.CodeSnippet == null)
                throw new Exception($"Session's CodeSnippet is null! SessionId: {session.Id}");

            if (session.CodeSnippet.Content != request.Content)
            {
                session.CodeSnippet.Content = request.Content;
                isChanged = true;
            }
        }

        SessionEditHistory? editHistory = null;

        if (isChanged || !string.IsNullOrWhiteSpace(request.Changes))
        {
            session.EditedAt = DateTime.UtcNow;

            editHistory = new SessionEditHistory
            {
                Id = Guid.NewGuid(),
                SessionId = session.Id,
                EditedByUserId = user.Id,
                EditedAt = DateTime.UtcNow,
                Changes = changesDesc.Trim()
            };
        }

        var updatedSession = await _sessionRepo.UpdateSessionAsync(session, editHistory);

        return _mapper.Map<CollabSessionDto>(updatedSession);
    }
}

