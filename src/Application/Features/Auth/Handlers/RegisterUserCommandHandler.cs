using Application.DTOs;
using Application.Features.Auth.Commands;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
namespace Application.Features.Auth.Handlers;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public RegisterUserCommandHandler(
        IUserRepository repo, 
        IMapper mapper, 
        IWebHostEnvironment env)
    {
        _repo = repo;
        _mapper = mapper;
        _env = env;
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
            CreatedAt = DateTime.UtcNow,
            IsAdmin = false,
            AvatarFileName = dto.AvatarUrl
        };

        if (request.Avatar != null && request.Avatar.Length > 0)
        {
            var avatarsPath = Path.Combine(_env.WebRootPath, "avatars");
            Directory.CreateDirectory(avatarsPath);

            var ext = Path.GetExtension(request.Avatar.FileName)?.ToLower() ?? ".png";
            var avatarFileName = $"{user.Id}{ext}";
            var filePath = Path.Combine(avatarsPath, avatarFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.Avatar.CopyToAsync(stream, ct);
            }

        }
        else
        {
            user.AvatarFileName = null; 
        }

        await _repo.AddAsync(user);
        return _mapper.Map<UserDto>(user);
    }
}
