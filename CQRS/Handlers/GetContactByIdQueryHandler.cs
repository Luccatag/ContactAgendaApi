using MediatR;
using AutoMapper;
using ContactAgendaApi.CQRS.Queries;
using ContactAgendaApi.DTOs;
using ContactAgendaApi.Interfaces;

namespace ContactAgendaApi.CQRS.Handlers;

/// <summary>
/// Handler for GetContactByIdQuery
/// </summary>
public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, ContactReadDto?>
{
    private readonly IContactService _contactService;
    private readonly IMapper _mapper;
    private readonly ILogger<GetContactByIdQueryHandler> _logger;

    public GetContactByIdQueryHandler(
        IContactService contactService,
        IMapper mapper,
        ILogger<GetContactByIdQueryHandler> logger)
    {
        _contactService = contactService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ContactReadDto?> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving contact with ID: {Id}", request.Id);

        var contact = await _contactService.GetByIdAsync(request.Id);

        if (contact == null)
        {
            _logger.LogWarning("Contact with ID: {Id} not found", request.Id);
            return null;
        }

        _logger.LogInformation("Successfully retrieved contact with ID: {Id}", request.Id);

        return _mapper.Map<ContactReadDto>(contact);
    }
}
