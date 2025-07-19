using Microsoft.AspNetCore.Mvc;
using ContactAgendaApi.Interfaces;
using ContactAgendaApi.DTOs;
using ContactAgendaApi.Models;
using AutoMapper;

namespace ContactAgendaApi.Controllers;

// API controller for managing contacts
[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IContactService _service;
    private readonly IMapper _mapper;

    // Constructor injects the contact service and AutoMapper
    public ContactsController(IContactService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    // GET: api/contacts
    // Returns all contacts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactReadDto>>> GetAll()
    {
        var contacts = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<ContactReadDto>>(contacts));
    }

    // GET: api/contacts/{id}
    // Returns a single contact by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<ContactReadDto>> GetById(int id)
    {
        var contact = await _service.GetByIdAsync(id);
        if (contact == null)
            return NotFound();
        return Ok(_mapper.Map<ContactReadDto>(contact));
    }

    // POST: api/contacts
    // Creates a new contact
    [HttpPost]
    public async Task<ActionResult<ContactReadDto>> Create(ContactCreateDto dto)
    {
        var contact = _mapper.Map<Contact>(dto);
        var created = await _service.CreateAsync(contact);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<ContactReadDto>(created));
    }

    // PUT: api/contacts/{id}
    // Updates an existing contact
    [HttpPut("{id}")]
    public async Task<ActionResult<ContactReadDto>> Update(int id, ContactUpdateDto dto)
    {
        var contact = _mapper.Map<Contact>(dto);
        var updated = await _service.UpdateAsync(id, contact);
        if (updated == null)
            return NotFound();
        return Ok(_mapper.Map<ContactReadDto>(updated));
    }

    // DELETE: api/contacts/{id}
    // Deletes a contact by ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}
