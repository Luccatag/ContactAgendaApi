using Microsoft.AspNetCore.Mvc;
using ContactAgendaApi.Interfaces;
using ContactAgendaApi.DTOs;
using ContactAgendaApi.Models;
using AutoMapper;

namespace ContactAgendaApi.Controllers;

// This attribute tells ASP.NET Core this is a Web API controller
[ApiController]
// This sets the route to e.g. /api/contacts or /api/contactagenda
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IContactService _service;
    private readonly IMapper _mapper;

    public ContactsController(IContactService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactReadDto>>> GetAll()
    {
        var contacts = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<ContactReadDto>>(contacts));
    }

    // GET: api/contacts/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ContactReadDto>> GetById(int id)
    {
        var contact = await _service.GetByIdAsync(id);
        if (contact == null)
            return NotFound();
        return Ok(_mapper.Map<ContactReadDto>(contact));
    }

    [HttpPost]
    public async Task<ActionResult<ContactReadDto>> Create(ContactCreateDto dto)
    {
        var contact = _mapper.Map<Contact>(dto);
        var created = await _service.CreateAsync(contact);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<ContactReadDto>(created));
    }

    // PUT: api/contacts/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<ContactReadDto>> Update(int id, ContactCreateDto dto)
    {
        var contact = _mapper.Map<Contact>(dto);
        contact.Id = id;
        var updated = await _service.UpdateAsync(contact);
        if (updated == null)
            return NotFound();
        return Ok(_mapper.Map<ContactReadDto>(updated));
    }

    // DELETE: api/contacts/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}
