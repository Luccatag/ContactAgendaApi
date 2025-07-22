using MediatR;

namespace ContactAgendaApi.CQRS.Commands;

/// <summary>
/// Command to delete a contact
/// </summary>
public record DeleteContactCommand(int Id) : IRequest<bool>;
