using MediatR;
using ContactAgendaApi.DTOs;

namespace ContactAgendaApi.CQRS.Commands;

/// <summary>
/// Command to create a new contact
/// </summary>
public record CreateContactCommand(
    string Name,
    string Email,
    string Phone,
    bool IsFavorite = false
) : IRequest<ContactReadDto>;
