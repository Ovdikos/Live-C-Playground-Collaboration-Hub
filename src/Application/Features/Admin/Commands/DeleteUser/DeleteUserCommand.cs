using MediatR;

namespace Application.Features.Admin.Commands.DeleteUser;

public record DeleteUserCommand(Guid UserId) : IRequest<bool>;