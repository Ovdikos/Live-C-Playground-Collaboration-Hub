using Application.DTOs;
using MediatR;

namespace Application.Features.Auth.Queries;

public record LoginUserQuery(LoginUserDto Dto) : IRequest<UserDto?>;