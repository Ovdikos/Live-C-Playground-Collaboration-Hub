using Application.DTOs;
using MediatR;

namespace Application.Features.Auth.Queries;

public record GetAllSessionsQuery
    (string? Search, bool? IsActive, DateTime? CreatedFrom, DateTime? CreatedTo, int? MinParticipants) : IRequest<List<CollabSessionDetailsDto>>;