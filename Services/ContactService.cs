using ContactAgendaApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContactAgendaApi.Interfaces;

namespace ContactAgendaApi.Services;

using Microsoft.EntityFrameworkCore;

public class ContactService : IContactService
{
    // Old EF Core context, now commented out:
    // private readonly AppDbContext _context;
    // public ContactService(AppDbContext context)
    // {
    //     _context = context;
    // }

    // New: Use IContactRepository for persistence (can be JSON, EF, etc.)
    private readonly IContactRepository _repository;
    public ContactService(IContactRepository repository)
    {
        _repository = repository;
    }

    // Now delegates to repository, which can be backed by JSON or DB
    public async Task<IEnumerable<Contact>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Contact?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Contact> CreateAsync(Contact contact)
    {
        return await _repository.CreateAsync(contact);
    }

    public async Task<Contact?> UpdateAsync(Contact updatedContact)
    {
        return await _repository.UpdateAsync(updatedContact);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
