using Application.DTOs.SessionDtos;
using MediatR;

namespace Application.Features.CollabSessions.Commands.JoinCollabSession;

public record JoinCollabSessionCommand(JoinSessionDto Dto) : IRequest;