using Application.DTOs;
using Application.Features.Auth.Commands;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Auth.Handlers;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;
    public RegisterUserCommandHandler(IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        if (await _repo.ExistsByUsernameOrEmail(request.Username, request.Email))
            throw new Exception("User already exists!");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password is required");

        var user = new User
        {
            UserName = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Email = request.Email,
            AvatarUrl = request.AvatarUrl,
            CreatedAt = DateTime.UtcNow,
            IsAdmin = true
        };
        await _repo.AddAsync(user);
        return _mapper.Map<UserDto>(user);
    }
}