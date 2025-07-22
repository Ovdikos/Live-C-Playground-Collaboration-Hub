using MediatR;

namespace Application.Features.Admin.Commands.DeleteCollabSession;

public record DeleteCollabSessionCommand(string Name) : IRequest<bool>;
