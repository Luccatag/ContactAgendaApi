using Microsoft.AspNetCore.Mvc;
using ContactAgendaApi.DTOs;
using ContactAgendaApi.Interfaces;
using ContactAgendaApi.Models;
using AutoMapper;

namespace ContactAgendaApi.Controllers;

/// <summary>
/// Simplified CQRS-based controller for contact management
/// Demonstrates CQRS pattern without MediatR dependency
/// Separates read and write operations for better scalability and maintainability
/// </summary>
[ApiController]
[Route("api/v2/[controller]")]
public class ContactsCqrsSimpleController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly IMapper _mapper;
    private readonly ILogger<ContactsCqrsSimpleController> _logger;

    public ContactsCqrsSimpleController(
        IContactService contactService, 
        IMapper mapper, 
        ILogger<ContactsCqrsSimpleController> logger)
    {
        _contactService = contactService;
        _mapper = mapper;
        _logger = logger;
    }

    // ===============================
    // QUERIES (READ OPERATIONS)
    // ===============================

    /// <summary>
    /// Query: Get all contacts
    /// </summary>
    /// <returns>List of all contacts</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ContactReadDto>))]
    public async Task<ActionResult<IEnumerable<ContactReadDto>>> GetAllContacts()
    {
        _logger.LogInformation("CQRS Query: Getting all contacts");
        
        var contacts = await _contactService.GetAllAsync();
        var result = _mapper.Map<IEnumerable<ContactReadDto>>(contacts);
        
        _logger.LogInformation("CQRS Query: Retrieved {ContactCount} contacts", result.Count());
        
        return Ok(result);
    }

    /// <summary>
    /// Query: Get a contact by ID
    /// </summary>
    /// <param name="id">Contact ID</param>
    /// <returns>Contact details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContactReadDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactReadDto>> GetContactById(int id)
    {
        _logger.LogInformation("CQRS Query: Getting contact with ID: {Id}", id);
        
        var contact = await _contactService.GetByIdAsync(id);
        
        if (contact == null)
        {
            _logger.LogWarning("CQRS Query: Contact with ID: {Id} not found", id);
            return NotFound($"Contact with ID {id} not found");
        }
        
        var result = _mapper.Map<ContactReadDto>(contact);
        _logger.LogInformation("CQRS Query: Successfully retrieved contact with ID: {Id}", id);
        
        return Ok(result);
    }

    /// <summary>
    /// Query: Search contacts by name, email, or phone
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <returns>List of matching contacts</returns>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ContactReadDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ContactReadDto>>> SearchContacts([FromQuery] string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest("Search term cannot be empty");
        }

        _logger.LogInformation("CQRS Query: Searching contacts with term: {SearchTerm}", searchTerm);
        
        // Get all contacts and filter by search term (CQRS Query pattern)
        var allContacts = await _contactService.GetAllAsync();
        
        var filteredContacts = allContacts.Where(c =>
            c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            c.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            c.Phone.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
        );

        var result = _mapper.Map<IEnumerable<ContactReadDto>>(filteredContacts);
        
        _logger.LogInformation("CQRS Query: Found {ContactCount} contacts matching search term: {SearchTerm}", 
            result.Count(), searchTerm);
        
        return Ok(result);
    }

    // ===============================
    // COMMANDS (WRITE OPERATIONS)
    // ===============================

    /// <summary>
    /// Command: Create a new contact
    /// </summary>
    /// <param name="dto">Contact creation data</param>
    /// <returns>Created contact</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ContactReadDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContactReadDto>> CreateContact([FromBody] ContactCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("CQRS Command: Invalid model state for contact creation");
            return BadRequest(ModelState);
        }

        _logger.LogInformation("CQRS Command: Creating new contact with email: {Email}", dto.Email);
        
        try
        {
            // Map DTO to domain model (CQRS Command pattern)
            var contact = new Contact
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                IsFavorite = dto.IsFavorite
            };

            var createdContact = await _contactService.CreateAsync(contact);

            if (createdContact == null)
            {
                _logger.LogError("CQRS Command: Failed to create contact with email: {Email}", dto.Email);
                return BadRequest("Failed to create contact");
            }

            var result = _mapper.Map<ContactReadDto>(createdContact);
            
            _logger.LogInformation("CQRS Command: Successfully created contact with ID: {Id}", result.Id);
            
            return CreatedAtAction(nameof(GetContactById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CQRS Command: Error creating contact");
            return BadRequest("Failed to create contact");
        }
    }

    /// <summary>
    /// Command: Update an existing contact
    /// </summary>
    /// <param name="id">Contact ID</param>
    /// <param name="dto">Contact update data</param>
    /// <returns>Updated contact</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContactReadDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactReadDto>> UpdateContact(int id, [FromBody] ContactUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("CQRS Command: Invalid model state for contact update");
            return BadRequest(ModelState);
        }

        _logger.LogInformation("CQRS Command: Updating contact with ID: {Id}", id);
        
        try
        {
            // Check if contact exists (CQRS Command pattern)
            var existingContact = await _contactService.GetByIdAsync(id);
            if (existingContact == null)
            {
                _logger.LogWarning("CQRS Command: Contact with ID: {Id} not found for update", id);
                return NotFound($"Contact with ID {id} not found");
            }

            // Update the existing contact with new values
            existingContact.Name = dto.Name;
            existingContact.Email = dto.Email;
            existingContact.Phone = dto.Phone;
            existingContact.IsFavorite = dto.IsFavorite;

            var updatedContact = await _contactService.UpdateAsync(existingContact);

            if (updatedContact == null)
            {
                _logger.LogError("CQRS Command: Failed to update contact with ID: {Id}", id);
                return BadRequest("Failed to update contact");
            }

            var result = _mapper.Map<ContactReadDto>(updatedContact);
            
            _logger.LogInformation("CQRS Command: Successfully updated contact with ID: {Id}", id);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CQRS Command: Error updating contact with ID: {Id}", id);
            return BadRequest("Failed to update contact");
        }
    }

    /// <summary>
    /// Command: Delete a contact
    /// </summary>
    /// <param name="id">Contact ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteContact(int id)
    {
        _logger.LogInformation("CQRS Command: Deleting contact with ID: {Id}", id);
        
        try
        {
            // Check if contact exists before deletion (CQRS Command pattern)
            var existingContact = await _contactService.GetByIdAsync(id);
            if (existingContact == null)
            {
                _logger.LogWarning("CQRS Command: Contact with ID: {Id} not found for deletion", id);
                return NotFound($"Contact with ID {id} not found");
            }

            var result = await _contactService.DeleteAsync(id);
            
            if (!result)
            {
                _logger.LogError("CQRS Command: Failed to delete contact with ID: {Id}", id);
                return BadRequest("Failed to delete contact");
            }
            
            _logger.LogInformation("CQRS Command: Successfully deleted contact with ID: {Id}", id);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CQRS Command: Error deleting contact with ID: {Id}", id);
            return BadRequest("Failed to delete contact");
        }
    }

    /// <summary>
    /// Command: Toggle favorite status of a contact
    /// </summary>
    /// <param name="id">Contact ID</param>
    /// <returns>Updated contact</returns>
    [HttpPatch("{id}/favorite")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContactReadDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactReadDto>> ToggleFavorite(int id)
    {
        _logger.LogInformation("CQRS Command: Toggling favorite status for contact with ID: {Id}", id);
        
        try
        {
            // Get existing contact (CQRS Command pattern)
            var existingContact = await _contactService.GetByIdAsync(id);
            if (existingContact == null)
            {
                _logger.LogWarning("CQRS Command: Contact with ID: {Id} not found for favorite toggle", id);
                return NotFound($"Contact with ID {id} not found");
            }

            // Toggle favorite status
            existingContact.IsFavorite = !existingContact.IsFavorite;

            var updatedContact = await _contactService.UpdateAsync(existingContact);

            if (updatedContact == null)
            {
                _logger.LogError("CQRS Command: Failed to toggle favorite status for contact with ID: {Id}", id);
                return BadRequest("Failed to toggle favorite status");
            }

            var result = _mapper.Map<ContactReadDto>(updatedContact);
            
            _logger.LogInformation("CQRS Command: Successfully toggled favorite status for contact with ID: {Id}. New status: {IsFavorite}", 
                id, updatedContact.IsFavorite);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CQRS Command: Error toggling favorite for contact with ID: {Id}", id);
            return BadRequest("Failed to toggle favorite status");
        }
    }
}
