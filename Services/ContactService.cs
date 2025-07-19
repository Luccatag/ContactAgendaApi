using ContactAgendaApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContactAgendaApi.Interfaces;

namespace ContactAgendaApi.Services;

public class ContactService : IContactService
{
    private readonly List<Contact> _contacts = new();
    private int _nextId = 1;

    public Task<IEnumerable<Contact>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Contact>>(_contacts);
    }

    public Task<Contact> CreateAsync(Contact contact)
    {
        contact.Id = _nextId++;
        _contacts.Add(contact);
        return Task.FromResult(contact);
    }
}
