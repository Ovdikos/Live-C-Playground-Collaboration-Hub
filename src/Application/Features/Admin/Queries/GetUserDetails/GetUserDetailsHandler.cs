using Application.DTOs.UserDtos;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Admin.Queries.GetUserDetails;

public class GetUserDetailsHandler : IRequestHandler<GetUserDetailsQuery, UserDetailsDto>
{
    private readonly IMapper _mapper;
    private readonly IAdminRepository _repo;

    public GetUserDetailsHandler(IAdminRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<UserDetailsDto> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var user = await _repo.GetUserDetailsByUsernameAsync(request.Username);
        if (user == null)
            return null!; 

        return _mapper.Map<UserDetailsDto>(user);
    }
}