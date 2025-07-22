using MediatR;
using ContactAgendaApi.CQRS.Commands;
using ContactAgendaApi.Interfaces;

namespace ContactAgendaApi.CQRS.Handlers;

/// <summary>
/// Handler for DeleteContactCommand
/// </summary>
public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, bool>
{
    private readonly IContactService _contactService;
    private readonly ILogger<DeleteContactCommandHandler> _logger;

    public DeleteContactCommandHandler(
        IContactService contactService,
        ILogger<DeleteContactCommandHandler> logger)
    {
        _contactService = contactService;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting contact with ID: {Id}", request.Id);

        // Check if contact exists
        var existingContact = await _contactService.GetByIdAsync(request.Id);
        if (existingContact == null)
        {
            _logger.LogWarning("Contact with ID: {Id} not found for deletion", request.Id);
            return false;
        }

        // Delete contact using the service
        var result = await _contactService.DeleteAsync(request.Id);

        if (result)
        {
            _logger.LogInformation("Successfully deleted contact with ID: {Id}", request.Id);
        }
        else
        {
            _logger.LogError("Failed to delete contact with ID: {Id}", request.Id);
        }

        return result;
    }
}
