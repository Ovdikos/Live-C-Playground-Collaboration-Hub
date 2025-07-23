using Application.DTOs.LoginRegisterDtos;
using Application.DTOs.UserDtos;
using Application.Features.Auth.Commands.RegisterUser;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests;

public class RegisterUserTests
{
    private readonly Mock<IUserRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly RegisterUserHandler _handler;

    public RegisterUserTests()
    {
        _handler = new RegisterUserHandler(_repo.Object, _mapper.Object, null!);
    }

    [Fact]
    public async Task Should_ThrowException_When_Username_Or_Email_AlreadyExists()
    {
        var dto = new RegisterUserDto
        {
            Username = "alex",
            Email = "alex@email.com",
            Password = "Test123!"
        };
        var command = new RegisterUserCommand { Dto = dto };

        _repo.Setup(r => r.ExistsByUsernameOrEmail(dto.Username, dto.Email)).ReturnsAsync(true);

        var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, default));
        ex.Message.Should().Contain("already exists");
    }

    [Fact]
    public async Task Should_ThrowArgumentException_When_Password_IsEmpty()
    {
        var dto = new RegisterUserDto
        {
            Username = "alex",
            Email = "alex@email.com",
            Password = ""
        };
        var command = new RegisterUserCommand { Dto = dto };

        _repo.Setup(r => r.ExistsByUsernameOrEmail(dto.Username, dto.Email)).ReturnsAsync(false);

        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Should_CreateUser_When_Data_IsValid()
    {
        var dto = new RegisterUserDto
        {
            Username = "newuser",
            Email = "newuser@email.com",
            Password = "Test123!"
        };
        var command = new RegisterUserCommand { Dto = dto };

        _repo.Setup(r => r.ExistsByUsernameOrEmail(dto.Username, dto.Email)).ReturnsAsync(false);
        _repo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _mapper.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
               .Returns(new UserDto { Username = "newuser", Email = "newuser@email.com" });

        var result = await _handler.Handle(command, default);

        result.Should().NotBeNull();
        result.Username.Should().Be("newuser");
        result.Email.Should().Be("newuser@email.com");
        _repo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Should_ThrowException_When_Username_IsNullOrWhitespace()
    {
        var dto = new RegisterUserDto
        {
            Username = "   ",
            Email = "user@email.com",
            Password = "Test123!"
        };
        var command = new RegisterUserCommand { Dto = dto };

        _repo.Setup(r => r.ExistsByUsernameOrEmail(dto.Username, dto.Email)).ReturnsAsync(false);

        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Should_Save_HashPassword_For_NewUser()
    {
        var dto = new RegisterUserDto
        {
            Username = "alex",
            Email = "alex@email.com",
            Password = "Test123!"
        };
        var command = new RegisterUserCommand { Dto = dto };

        User? createdUser = null;
        _repo.Setup(r => r.ExistsByUsernameOrEmail(dto.Username, dto.Email)).ReturnsAsync(false);
        _repo.Setup(r => r.AddAsync(It.IsAny<User>())).Callback<User>(u => createdUser = u).Returns(Task.CompletedTask);
        _mapper.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
            .Returns(new UserDto { Username = "alex", Email = "alex@email.com" });

        var result = await _handler.Handle(command, default);

        createdUser.Should().NotBeNull();
        createdUser.PasswordHash.Should().NotBeNullOrEmpty();
        createdUser.PasswordHash.Should().NotBe(dto.Password);
        BCrypt.Net.BCrypt.Verify("Test123!", createdUser.PasswordHash).Should().BeTrue();
    }

    [Fact]
    public async Task Should_Set_DefaultAvatar_When_Avatar_IsNotUploaded()
    {
        var dto = new RegisterUserDto
        {
            Username = "avatarless",
            Email = "avatarless@email.com",
            Password = "Test123!"
        };
        var command = new RegisterUserCommand { Dto = dto, Avatar = null };

        User? createdUser = null;
        _repo.Setup(r => r.ExistsByUsernameOrEmail(dto.Username, dto.Email)).ReturnsAsync(false);
        _repo.Setup(r => r.AddAsync(It.IsAny<User>())).Callback<User>(u => createdUser = u).Returns(Task.CompletedTask);
        _mapper.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
            .Returns(new UserDto { Username = "avatarless", Email = "avatarless@email.com" });

        var result = await _handler.Handle(command, default);

        createdUser.Should().NotBeNull();
        createdUser.AvatarFileName.Should().BeNull();
    }
}