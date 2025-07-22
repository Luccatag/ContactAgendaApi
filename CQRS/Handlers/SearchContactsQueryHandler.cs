using MediatR;
using AutoMapper;
using ContactAgendaApi.CQRS.Queries;
using ContactAgendaApi.DTOs;
using ContactAgendaApi.Interfaces;

namespace ContactAgendaApi.CQRS.Handlers;

/// <summary>
/// Handler for SearchContactsQuery
/// </summary>
public class SearchContactsQueryHandler : IRequestHandler<SearchContactsQuery, IEnumerable<ContactReadDto>>
{
    private readonly IContactService _contactService;
    private readonly IMapper _mapper;
    private readonly ILogger<SearchContactsQueryHandler> _logger;

    public SearchContactsQueryHandler(
        IContactService contactService,
        IMapper mapper,
        ILogger<SearchContactsQueryHandler> logger)
    {
        _contactService = contactService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ContactReadDto>> Handle(SearchContactsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching contacts with term: {SearchTerm}", request.SearchTerm);

        // Get all contacts and filter by search term
        var allContacts = await _contactService.GetAllAsync();
        
        var filteredContacts = allContacts.Where(c =>
            c.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
            c.Email.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
            c.Phone.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)
        );

        _logger.LogInformation("Found {ContactCount} contacts matching search term: {SearchTerm}", 
            filteredContacts.Count(), request.SearchTerm);

        return _mapper.Map<IEnumerable<ContactReadDto>>(filteredContacts);
    }
}
