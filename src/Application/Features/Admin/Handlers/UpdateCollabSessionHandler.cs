using Application.Features.Admin.Commands;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Admin.Handlers;

public class UpdateCollabSessionHandler : IRequestHandler<UpdateCollabSessionCommand, bool>
{
    private readonly IAdminRepository _repo;
    private readonly IMapper _mapper;

    public UpdateCollabSessionHandler(IAdminRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateCollabSessionCommand request, CancellationToken ct)
    {
        var session = await _repo.GetByIdAsync(request.Dto.Id);
        if (session == null) return false;

        _mapper.Map(request.Dto, session);

        return await _repo.UpdateAsync(session);
    }
}