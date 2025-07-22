using MediatR;
using ContactAgendaApi.DTOs;

namespace ContactAgendaApi.CQRS.Queries;

/// <summary>
/// Query to get all contacts
/// </summary>
public record GetAllContactsQuery() : IRequest<IEnumerable<ContactReadDto>>;
