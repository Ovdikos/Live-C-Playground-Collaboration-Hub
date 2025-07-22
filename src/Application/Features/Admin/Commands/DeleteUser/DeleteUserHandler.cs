using Core.Interfaces;
using MediatR;

namespace Application.Features.Admin.Commands.DeleteUser;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IAdminRepository _repo;
    public DeleteUserHandler(IAdminRepository repo) { _repo = repo; }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        return await _repo.DeleteUserAsync(request.UserId);
    }
}