using Application.DTOs.SessionDtos;
using MediatR;

namespace Application.Features.Admin.Commands.UpdateCollabSession;

public record UpdateCollabSessionCommand(CollabSessionEditDto Dto) : IRequest<bool>;