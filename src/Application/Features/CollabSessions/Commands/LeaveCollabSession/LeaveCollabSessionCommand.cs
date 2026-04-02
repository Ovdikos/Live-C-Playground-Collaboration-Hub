using Application.DTOs.SessionDtos;
using MediatR;

namespace Application.Features.CollabSessions.Commands.LeaveCollabSession;

public record LeaveCollabSessionCommand(LeaveSessionDto Dto) : IRequest;
