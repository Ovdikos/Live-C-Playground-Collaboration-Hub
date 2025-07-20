using MediatR;

namespace Application.Features.Admin.Commands;

public record DeleteCollabSessionCommand(string Name) : IRequest<bool>;
