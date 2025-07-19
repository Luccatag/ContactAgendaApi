using ContactAgendaApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContactAgendaApi.Interfaces;

namespace ContactAgendaApi.Services;

using Microsoft.EntityFrameworkCore;

public class ContactService : IContactService
{
    private readonly AppDbContext _context;
    public ContactService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Contact>> GetAllAsync()
    {
        return await _context.Contacts.AsNoTracking().ToListAsync();
    }

    public async Task<Contact?> GetByIdAsync(int id)
    {
        return await _context.Contacts.FindAsync(id);
    }

    public async Task<Contact> CreateAsync(Contact contact)
    {
        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();
        return contact;
    }

    public async Task<Contact?> UpdateAsync(Contact updatedContact)
    {
        var contact = await _context.Contacts.FindAsync(updatedContact.Id);
        if (contact == null)
            return null;
        contact.Name = updatedContact.Name;
        contact.Email = updatedContact.Email;
        contact.Phone = updatedContact.Phone;
        await _context.SaveChangesAsync();
        return contact;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
            return false;
        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();
        return true;
    }
}
