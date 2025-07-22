using Application.DTOs.LoginRegisterDtos;
using Application.DTOs.UserDtos;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Auth.Queries.LoginUser;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, LoginResultDto>
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;

    public LoginUserQueryHandler(IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<LoginResultDto> Handle(LoginUserQuery request, CancellationToken ct)
    {
        var dto = request.Dto;
        var user = await _repo.GetByUsernameOrEmailAsync(dto.Login);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return new LoginResultDto { Success = false, Error = "Invalid credentials." };

        if (user.IsBlocked)
        {
            return new LoginResultDto
            {
                Success = false,
                Error = $"Your account was blocked, please contact {user.BlockedByAdminEmail ?? "administrator"} to figure this out"
            };
        }

        return new LoginResultDto
        {
            Success = true,
            User = _mapper.Map<UserDto>(user)
        };
    }
}