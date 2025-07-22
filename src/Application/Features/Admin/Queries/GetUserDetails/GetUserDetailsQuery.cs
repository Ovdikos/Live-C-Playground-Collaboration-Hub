using Application.DTOs.UserDtos;
using MediatR;

namespace Application.Features.Admin.Queries.GetUserDetails;

public record GetUserDetailsQuery(string Username) : IRequest<UserDetailsDto>;
