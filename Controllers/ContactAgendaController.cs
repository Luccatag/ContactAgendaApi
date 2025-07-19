using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ContactAgendaApi.Models;

namespace ContactAgendaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactAgendaController : ControllerBase
{
    // In-memory list for demonstration
    private static readonly List<Contact> Contacts = new();
    private static int _nextId = 1;

    // GET: api/contactagenda
    [HttpGet]
    public ActionResult<IEnumerable<Contact>> GetAll()
    {
        return Ok(Contacts);
    }

    // GET: api/contactagenda/{id}
    [HttpGet("{id}")]
    public ActionResult<Contact> GetById(int id)
    {
        var contact = Contacts.FirstOrDefault(c => c.Id == id);
        if (contact == null)
            return NotFound();
        return Ok(contact);
    }

    // POST: api/contactagenda
    [HttpPost]
    public ActionResult<Contact> Create(Contact contact)
    {
        contact.Id = _nextId++;
        Contacts.Add(contact);
        return CreatedAtAction(nameof(GetById), new { id = contact.Id }, contact);
    }

    // PUT: api/contactagenda/{id}
    [HttpPut("{id}")]
    public ActionResult<Contact> Update(int id, Contact updatedContact)
    {
        var contact = Contacts.FirstOrDefault(c => c.Id == id);
        if (contact == null)
            return NotFound();
        contact.Name = updatedContact.Name;
        contact.Email = updatedContact.Email;
        contact.Phone = updatedContact.Phone;
        return Ok(contact);
    }

    // DELETE: api/contactagenda/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var contact = Contacts.FirstOrDefault(c => c.Id == id);
        if (contact == null)
            return NotFound();
        Contacts.Remove(contact);
        return NoContent();
    }
}
