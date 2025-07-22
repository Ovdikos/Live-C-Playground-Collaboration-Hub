using Application.DTOs.SessionDtos;
using MediatR;

namespace Application.Features.Admin.Queries.GetCollabSessionDetails;

public record GetCollabSessionDetailsQuery(string Name) : IRequest<CollabSessionDetailsDto?>;
