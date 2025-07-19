using Microsoft.AspNetCore.Mvc;
using ContactAgendaApi.Interfaces;
using ContactAgendaApi.DTOs;
using ContactAgendaApi.Models;
using AutoMapper;

namespace ContactAgendaApi.Controllers;

[ApiController]
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

    [HttpPost]
    public async Task<ActionResult<ContactReadDto>> Create(ContactCreateDto dto)
    {
        var contact = _mapper.Map<Contact>(dto);
        var created = await _service.CreateAsync(contact);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, _mapper.Map<ContactReadDto>(created));
    }
}
