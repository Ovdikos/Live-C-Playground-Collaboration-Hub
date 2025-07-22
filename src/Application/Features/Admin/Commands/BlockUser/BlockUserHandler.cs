using Core.Interfaces;
using MediatR;

namespace Application.Features.Admin.Commands.BlockUser;

public class BlockUserHandler : IRequestHandler<BlockUserCommand, bool>
{
    private readonly IAdminRepository _repo;
    public BlockUserHandler(IAdminRepository repo) { _repo = repo; }

    public async Task<bool> Handle(BlockUserCommand request, CancellationToken ct)
    {
        return await _repo.SetUserBlockedAsync(request.UserId, request.Block, request.AdminEmail);
    }
}