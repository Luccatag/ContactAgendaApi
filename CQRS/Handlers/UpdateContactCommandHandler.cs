using MediatR;
using AutoMapper;
using ContactAgendaApi.CQRS.Commands;
using ContactAgendaApi.DTOs;
using ContactAgendaApi.Interfaces;

namespace ContactAgendaApi.CQRS.Handlers;

/// <summary>
/// Handler for UpdateContactCommand
/// </summary>
public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, ContactReadDto>
{
    private readonly IContactService _contactService;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateContactCommandHandler> _logger;

    public UpdateContactCommandHandler(
        IContactService contactService,
        IMapper mapper,
        ILogger<UpdateContactCommandHandler> logger)
    {
        _contactService = contactService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ContactReadDto> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating contact with ID: {Id}", request.Id);

        // Check if contact exists
        var existingContact = await _contactService.GetByIdAsync(request.Id);
        if (existingContact == null)
        {
            _logger.LogWarning("Contact with ID: {Id} not found for update", request.Id);
            throw new KeyNotFoundException($"Contact with ID {request.Id} not found");
        }

        // Update the existing contact with new values
        existingContact.Name = request.Name;
        existingContact.Email = request.Email;
        existingContact.Phone = request.Phone;
        existingContact.IsFavorite = request.IsFavorite;

        // Update contact using the service
        var updatedContact = await _contactService.UpdateAsync(existingContact);

        if (updatedContact == null)
        {
            _logger.LogError("Failed to update contact with ID: {Id}", request.Id);
            throw new InvalidOperationException($"Failed to update contact with ID {request.Id}");
        }

        _logger.LogInformation("Successfully updated contact with ID: {Id}", request.Id);

        // Map to read DTO and return
        return _mapper.Map<ContactReadDto>(updatedContact);
    }
}
