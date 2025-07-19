using Application.DTOs;
using MediatR;

namespace Application.Features.Admin.Queries;

public record GetUserDetailsQuery(string Username) : IRequest<UserDetailsDto>;
