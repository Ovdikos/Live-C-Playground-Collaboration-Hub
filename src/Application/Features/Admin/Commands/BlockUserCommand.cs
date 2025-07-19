using MediatR;

namespace Application.Features.Admin.Commands;

public record BlockUserCommand(Guid UserId, bool Block, string? AdminEmail) : IRequest<bool>;