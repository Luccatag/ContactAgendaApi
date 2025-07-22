using Microsoft.AspNetCore.Mvc;
using MediatR;
using ContactAgendaApi.CQRS.Commands;
using ContactAgendaApi.CQRS.Queries;
using ContactAgendaApi.DTOs;

namespace ContactAgendaApi.Controllers;

/// <summary>
/// CQRS-based controller for contact management using MediatR pattern
/// Separates read and write operations for better scalability and maintainability
/// </summary>
[ApiController]
[Route("api/v2/[controller]")]
public class ContactsCqrsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ContactsCqrsController> _logger;

    public ContactsCqrsController(IMediator mediator, ILogger<ContactsCqrsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all contacts
    /// </summary>
    /// <returns>List of all contacts</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ContactReadDto>))]
    public async Task<ActionResult<IEnumerable<ContactReadDto>>> GetAll()
    {
        _logger.LogInformation("CQRS: Getting all contacts");
        
        var query = new GetAllContactsQuery();
        var result = await _mediator.Send(query);
        
        return Ok(result);
    }

    /// <summary>
    /// Get a contact by ID
    /// </summary>
    /// <param name="id">Contact ID</param>
    /// <returns>Contact details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContactReadDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactReadDto>> GetById(int id)
    {
        _logger.LogInformation("CQRS: Getting contact with ID: {Id}", id);
        
        var query = new GetContactByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (result == null)
        {
            _logger.LogWarning("CQRS: Contact with ID: {Id} not found", id);
            return NotFound($"Contact with ID {id} not found");
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Search contacts by name, email, or phone
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <returns>List of matching contacts</returns>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ContactReadDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ContactReadDto>>> Search([FromQuery] string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest("Search term cannot be empty");
        }

        _logger.LogInformation("CQRS: Searching contacts with term: {SearchTerm}", searchTerm);
        
        var query = new SearchContactsQuery(searchTerm);
        var result = await _mediator.Send(query);
        
        return Ok(result);
    }

    /// <summary>
    /// Create a new contact
    /// </summary>
    /// <param name="dto">Contact creation data</param>
    /// <returns>Created contact</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ContactReadDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContactReadDto>> Create([FromBody] ContactCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("CQRS: Invalid model state for contact creation");
            return BadRequest(ModelState);
        }

        _logger.LogInformation("CQRS: Creating new contact with email: {Email}", dto.Email);
        
        var command = new CreateContactCommand(dto.Name, dto.Email, dto.Phone, dto.IsFavorite);
        
        try
        {
            var result = await _mediator.Send(command);
            
            _logger.LogInformation("CQRS: Successfully created contact with ID: {Id}", result.Id);
            
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "CQRS: Failed to create contact");
            return BadRequest("Failed to create contact");
        }
    }

    /// <summary>
    /// Update an existing contact
    /// </summary>
    /// <param name="id">Contact ID</param>
    /// <param name="dto">Contact update data</param>
    /// <returns>Updated contact</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContactReadDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactReadDto>> Update(int id, [FromBody] ContactUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("CQRS: Invalid model state for contact update");
            return BadRequest(ModelState);
        }

        _logger.LogInformation("CQRS: Updating contact with ID: {Id}", id);
        
        var command = new UpdateContactCommand(id, dto.Name, dto.Email, dto.Phone, dto.IsFavorite);
        
        try
        {
            var result = await _mediator.Send(command);
            
            _logger.LogInformation("CQRS: Successfully updated contact with ID: {Id}", id);
            
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "CQRS: Contact with ID: {Id} not found for update", id);
            return NotFound($"Contact with ID {id} not found");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "CQRS: Failed to update contact with ID: {Id}", id);
            return BadRequest("Failed to update contact");
        }
    }

    /// <summary>
    /// Delete a contact
    /// </summary>
    /// <param name="id">Contact ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogInformation("CQRS: Deleting contact with ID: {Id}", id);
        
        var command = new DeleteContactCommand(id);
        var result = await _mediator.Send(command);
        
        if (!result)
        {
            _logger.LogWarning("CQRS: Contact with ID: {Id} not found for deletion", id);
            return NotFound($"Contact with ID {id} not found");
        }
        
        _logger.LogInformation("CQRS: Successfully deleted contact with ID: {Id}", id);
        
        return NoContent();
    }

    /// <summary>
    /// Toggle favorite status of a contact
    /// </summary>
    /// <param name="id">Contact ID</param>
    /// <returns>Updated contact</returns>
    [HttpPatch("{id}/favorite")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContactReadDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactReadDto>> ToggleFavorite(int id)
    {
        _logger.LogInformation("CQRS: Toggling favorite status for contact with ID: {Id}", id);
        
        var command = new ToggleFavoriteCommand(id);
        
        try
        {
            var result = await _mediator.Send(command);
            
            _logger.LogInformation("CQRS: Successfully toggled favorite status for contact with ID: {Id}", id);
            
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "CQRS: Contact with ID: {Id} not found for favorite toggle", id);
            return NotFound($"Contact with ID {id} not found");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "CQRS: Failed to toggle favorite status for contact with ID: {Id}", id);
            return BadRequest("Failed to toggle favorite status");
        }
    }
}
