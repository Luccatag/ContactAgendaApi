using MediatR;
using AutoMapper;
using ContactAgendaApi.CQRS.Commands;
using ContactAgendaApi.DTOs;
using ContactAgendaApi.Interfaces;

namespace ContactAgendaApi.CQRS.Handlers;

/// <summary>
/// Handler for ToggleFavoriteCommand
/// </summary>
public class ToggleFavoriteCommandHandler : IRequestHandler<ToggleFavoriteCommand, ContactReadDto>
{
    private readonly IContactService _contactService;
    private readonly IMapper _mapper;
    private readonly ILogger<ToggleFavoriteCommandHandler> _logger;

    public ToggleFavoriteCommandHandler(
        IContactService contactService,
        IMapper mapper,
        ILogger<ToggleFavoriteCommandHandler> logger)
    {
        _contactService = contactService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ContactReadDto> Handle(ToggleFavoriteCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Toggling favorite status for contact with ID: {Id}", request.Id);

        // Get existing contact
        var existingContact = await _contactService.GetByIdAsync(request.Id);
        if (existingContact == null)
        {
            _logger.LogWarning("Contact with ID: {Id} not found for favorite toggle", request.Id);
            throw new KeyNotFoundException($"Contact with ID {request.Id} not found");
        }

        // Toggle favorite status
        existingContact.IsFavorite = !existingContact.IsFavorite;

        // Update contact using the service
        var updatedContact = await _contactService.UpdateAsync(existingContact);

        if (updatedContact == null)
        {
            _logger.LogError("Failed to toggle favorite status for contact with ID: {Id}", request.Id);
            throw new InvalidOperationException($"Failed to update contact with ID {request.Id}");
        }

        _logger.LogInformation("Successfully toggled favorite status for contact with ID: {Id}. New status: {IsFavorite}", 
            request.Id, updatedContact.IsFavorite);

        // Map to read DTO and return
        return _mapper.Map<ContactReadDto>(updatedContact);
    }
}
