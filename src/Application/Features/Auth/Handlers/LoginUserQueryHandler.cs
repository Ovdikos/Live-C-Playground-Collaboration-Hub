using Application.DTOs;
using Application.Features.Auth.Queries;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Auth.Handlers;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, UserDto?>
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;
    public LoginUserQueryHandler(IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(LoginUserQuery request, CancellationToken ct)
    {
        var dto = request.Dto;
        
        var user = await _repo.GetByUsernameAsync(dto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;
        return _mapper.Map<UserDto>(user);
    }
}