using MediatR;
using ContactAgendaApi.DTOs;

namespace ContactAgendaApi.CQRS.Commands;

/// <summary>
/// Command to update an existing contact
/// </summary>
public record UpdateContactCommand(
    int Id,
    string Name,
    string Email,
    string Phone,
    bool IsFavorite
) : IRequest<ContactReadDto>;
