using Application.DTOs;
using MediatR;

namespace Application.Features.Admin.Queries;

public record GetCollabSessionDetailsQuery(string Name) : IRequest<CollabSessionDetailsDto?>;
