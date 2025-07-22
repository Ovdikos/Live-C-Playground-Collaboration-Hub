using MediatR;

namespace Application.Features.Admin.Commands.BlockUser;

public record BlockUserCommand(Guid UserId, bool Block, string? AdminEmail) : IRequest<bool>;