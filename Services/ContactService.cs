using ContactAgendaApi.Models;
using ContactAgendaApi.Interfaces;

namespace ContactAgendaApi.Services;

// Implementation of IContactService (business logic)
public class ContactService : IContactService
{
    private readonly IContactRepository _repository;
    public ContactService(IContactRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Contact>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<Contact?> GetByIdAsync(int id)
        => await _repository.GetByIdAsync(id);

    public async Task<Contact> CreateAsync(Contact contact)
    {
        await _repository.AddAsync(contact);
        return contact;
    }

    public async Task<Contact?> UpdateAsync(int id, Contact contact)
    {
        if (!await _repository.ExistsAsync(id))
            return null;
        contact.Id = id;
        await _repository.UpdateAsync(contact);
        return contact;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var contact = await _repository.GetByIdAsync(id);
        if (contact == null)
            return false;
        await _repository.DeleteAsync(contact);
        return true;
    }
}
