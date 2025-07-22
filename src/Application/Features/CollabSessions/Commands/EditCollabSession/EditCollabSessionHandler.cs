using Application.DTOs.SessionDtos;
using AutoMapper;
using Core.Entities;
using Infrastructure.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CollabSessions.Commands.EditCollabSession;

public class EditCollabSessionHandler : IRequestHandler<EditCollabSessionCommand, CollabSessionDto>
{
    private readonly LivePlaygroundDbContext _context;
    private readonly IMapper _mapper;

    public EditCollabSessionHandler(LivePlaygroundDbContext db, IMapper mapper)
    {
        _context = db;
        _mapper = mapper;
    }

    public async Task<CollabSessionDto> Handle(EditCollabSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _context.CollabSessions
            .Include(x => x.CodeSnippet)
            .Include(x => x.Owner)
            .Include(x => x.EditHistories)
            .FirstOrDefaultAsync(x => x.Id == request.SessionId, cancellationToken);
        
        if (!session.IsActive) throw new UnauthorizedAccessException("SessionDtos is not active");
        
        if (session == null)
            throw new Exception($"SessionDtos not found! Id: {request.SessionId}");

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.EditedByUserId, cancellationToken);

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
                throw new Exception($"SessionDtos's CodeSnippet is null! SessionId: {session.Id}");

            if (session.CodeSnippet.Content != request.Content)
            {
                session.CodeSnippet.Content = request.Content;
                isChanged = true;
            }
        }

        if (isChanged || !string.IsNullOrWhiteSpace(request.Changes))
        {
            session.EditedAt = DateTime.UtcNow;

            var editHistory = new SessionEditHistory
            {
                Id = Guid.NewGuid(),
                SessionId = session.Id,
                EditedByUserId = user.Id,
                EditedAt = DateTime.UtcNow,
                Changes = changesDesc.Trim()
            };
            _context.Set<SessionEditHistory>().Add(editHistory);
        }

        await _context.SaveChangesAsync(cancellationToken);

        var fullSession = await _context.CollabSessions
            .Include(x => x.CodeSnippet)
            .Include(x => x.Owner)
            .Include(x => x.EditHistories)
                .ThenInclude(h => h.EditedByUser)
            .FirstOrDefaultAsync(x => x.Id == session.Id, cancellationToken);

        return _mapper.Map<CollabSessionDto>(fullSession!);
    }
}

