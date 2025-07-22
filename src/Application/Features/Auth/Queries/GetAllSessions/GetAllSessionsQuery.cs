using Application.DTOs.SessionDtos;
using MediatR;

namespace Application.Features.Auth.Queries.GetAllSessions;

public record GetAllSessionsQuery
    (string? Search, bool? IsActive, DateTime? CreatedFrom, DateTime? CreatedTo, int? MinParticipants) : IRequest<List<CollabSessionDetailsDto>>;