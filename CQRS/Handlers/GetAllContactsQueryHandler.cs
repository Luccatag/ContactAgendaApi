using MediatR;
using AutoMapper;
using ContactAgendaApi.CQRS.Queries;
using ContactAgendaApi.DTOs;
using ContactAgendaApi.Interfaces;

namespace ContactAgendaApi.CQRS.Handlers;

/// <summary>
/// Handler for GetAllContactsQuery
/// </summary>
public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, IEnumerable<ContactReadDto>>
{
    private readonly IContactService _contactService;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllContactsQueryHandler> _logger;

    public GetAllContactsQueryHandler(
        IContactService contactService,
        IMapper mapper,
        ILogger<GetAllContactsQueryHandler> logger)
    {
        _contactService = contactService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ContactReadDto>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving all contacts");

        var contacts = await _contactService.GetAllAsync();
        
        _logger.LogInformation("Retrieved {ContactCount} contacts", contacts.Count());

        return _mapper.Map<IEnumerable<ContactReadDto>>(contacts);
    }
}
