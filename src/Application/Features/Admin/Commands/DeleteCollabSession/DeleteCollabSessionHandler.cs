using Core.Interfaces;
using MediatR;

namespace Application.Features.Admin.Commands.DeleteCollabSession;

public class DeleteCollabSessionHandler : IRequestHandler<DeleteCollabSessionCommand, bool>
{
    private readonly IAdminRepository _repo;

    public DeleteCollabSessionHandler(IAdminRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> Handle(DeleteCollabSessionCommand request, CancellationToken ct)
    {
        return await _repo.DeleteSessionByNameAsync(request.Name);
    }
}