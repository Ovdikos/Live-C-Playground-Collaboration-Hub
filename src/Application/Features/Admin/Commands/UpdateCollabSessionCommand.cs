using Application.DTOs;
using MediatR;

namespace Application.Features.Admin.Commands;

public record UpdateCollabSessionCommand(CollabSessionEditDto Dto) : IRequest<bool>;