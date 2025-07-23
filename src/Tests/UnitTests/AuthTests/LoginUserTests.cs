using Application.DTOs.LoginRegisterDtos;
using Application.DTOs.UserDtos;
using Application.Features.Auth.Queries.LoginUser;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.AuthTests;

public class LoginUserTests
{
    private readonly Mock<IUserRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly LoginUserQueryHandler _handler;

    public LoginUserTests()
    {
        _handler = new LoginUserQueryHandler(_repo.Object, _mapper.Object);
    }

    [Fact]
    public async Task Should_ReturnUser_When_CredentialsAreValid()
    {
        var password = "Test123!";
        var user = new User
        {
            Username = "alex",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Email = "alex@mail.com"
        };

        var userDto = new UserDto { Username = user.Username, Email = user.Email };

        _repo.Setup(r => r.GetByUsernameOrEmailAsync("alex")).ReturnsAsync(user);
        _mapper.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

        var query = new LoginUserQuery
        {
            Dto = new LoginUserDto { Login = "alex", Password = password }
        };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Success.Should().BeTrue();
        result.User.Should().NotBeNull();
        result.User.Username.Should().Be("alex");
    }

    [Fact]
    public async Task Should_ReturnError_When_UsernameNotFound()
    {
        _repo.Setup(r => r.GetByUsernameOrEmailAsync("notfound")).ReturnsAsync((User)null!);

        var query = new LoginUserQuery
        {
            Dto = new LoginUserDto { Login = "notfound", Password = "pass" }
        };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Success.Should().BeFalse();
        result.Error.Should().Contain("Invalid credentials");
    }

    [Fact]
    public async Task Should_ReturnError_When_PasswordIncorrect()
    {
        var user = new User
        {
            Username = "alex",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("rightpassword"),
            Email = "alex@mail.com"
        };

        _repo.Setup(r => r.GetByUsernameOrEmailAsync("alex")).ReturnsAsync(user);

        var query = new LoginUserQuery
        {
            Dto = new LoginUserDto { Login = "alex", Password = "wrongpassword" }
        };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Success.Should().BeFalse();
        result.Error.Should().Contain("Invalid credentials");
    }

    [Fact]
    public async Task Should_ReturnError_When_UserBlocked()
    {
        var user = new User
        {
            Username = "alex",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123!"),
            Email = "alex@mail.com",
            IsBlocked = true,
            BlockedByAdminEmail = "admin@email.com"
        };

        _repo.Setup(r => r.GetByUsernameOrEmailAsync("alex")).ReturnsAsync(user);

        var query = new LoginUserQuery
        {
            Dto = new LoginUserDto { Login = "alex", Password = "Test123!" }
        };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Success.Should().BeFalse();
        result.Error.Should().Contain("blocked").And.Contain("admin@email.com");
    }

    [Fact]
    public async Task Should_ReturnError_When_UsernameIsEmpty()
    {
        var query = new LoginUserQuery
        {
            Dto = new LoginUserDto { Login = "", Password = "any" }
        };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Should_ReturnError_When_PasswordIsEmpty()
    {
        var query = new LoginUserQuery
        {
            Dto = new LoginUserDto { Login = "alex", Password = "" }
        };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Should_ReturnUser_When_LoginByEmail_IsSupported()
    {
        var password = "Test123!";
        var user = new User
        {
            Username = "alex",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Email = "alex@mail.com"
        };

        var userDto = new UserDto { Username = user.Username, Email = user.Email };

        _repo.Setup(r => r.GetByUsernameOrEmailAsync("alex@mail.com")).ReturnsAsync(user);
        _mapper.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

        var query = new LoginUserQuery
        {
            Dto = new LoginUserDto { Login = "alex@mail.com", Password = password }
        };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Success.Should().BeTrue();
        result.User.Should().NotBeNull();
    }
}