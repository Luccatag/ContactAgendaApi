using MediatR;
using ContactAgendaApi.DTOs;

namespace ContactAgendaApi.CQRS.Queries;

/// <summary>
/// Query to search contacts by name or email
/// </summary>
public record SearchContactsQuery(string SearchTerm) : IRequest<IEnumerable<ContactReadDto>>;
