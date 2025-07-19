using MediatR;

namespace Application.Features.Admin.Commands;

public record DeleteUserCommand(Guid UserId) : IRequest<bool>;