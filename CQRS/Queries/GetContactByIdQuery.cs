using MediatR;
using ContactAgendaApi.DTOs;

namespace ContactAgendaApi.CQRS.Queries;

/// <summary>
/// Query to get a contact by ID
/// </summary>
public record GetContactByIdQuery(int Id) : IRequest<ContactReadDto?>;
