using MediatR;
using AutoMapper;
using ContactAgendaApi.CQRS.Commands;
using ContactAgendaApi.DTOs;
using ContactAgendaApi.Interfaces;
using ContactAgendaApi.Models;

namespace ContactAgendaApi.CQRS.Handlers;

/// <summary>
/// Handler for CreateContactCommand
/// </summary>
public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, ContactReadDto>
{
    private readonly IContactService _contactService;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateContactCommandHandler> _logger;

    public CreateContactCommandHandler(
        IContactService contactService,
        IMapper mapper,
        ILogger<CreateContactCommandHandler> logger)
    {
        _contactService = contactService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ContactReadDto> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new contact with email: {Email}", request.Email);

        // Map command to contact model
        var contact = new Contact
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            IsFavorite = request.IsFavorite
        };

        // Create contact using the service
        var createdContact = await _contactService.CreateAsync(contact);

        if (createdContact == null)
        {
            _logger.LogError("Failed to create contact with email: {Email}", request.Email);
            throw new InvalidOperationException("Failed to create contact");
        }

        _logger.LogInformation("Successfully created contact with ID: {Id}", createdContact.Id);

        // Map to read DTO and return
        return _mapper.Map<ContactReadDto>(createdContact);
    }
}
