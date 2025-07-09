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
        var dto = request.Dto;
        if (await _repo.ExistsByUsernameOrEmail(dto.Username, dto.Email))
            throw new Exception("User already exists!");

        if (string.IsNullOrWhiteSpace(dto.Password))
            throw new ArgumentException("Password is required");
        
        var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            PasswordHash = hash,
            Email = dto.Email,
            AvatarUrl = dto.AvatarUrl,
            CreatedAt = DateTime.Now,
            IsAdmin = true
        };
        await _repo.AddAsync(user);
        return _mapper.Map<UserDto>(user);
    }
}