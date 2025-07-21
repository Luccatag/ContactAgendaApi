using ContactAgendaApi.Interfaces;
using ContactAgendaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactAgendaApi.Repositories;

/// <summary>
/// Entity Framework Core implementation of the contact repository
/// Provides CRUD operations using SQLite database through EF Core
/// </summary>
public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _context;

    public ContactRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Contact>> GetAllAsync()
    {
        return await _context.Contacts
            .OrderByDescending(c => c.IsFavorite)
            .ThenBy(c => c.Name)
            .ToListAsync();
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
        var existingContact = await _context.Contacts.FindAsync(updatedContact.Id);
        if (existingContact == null)
        {
            return null;
        }

        existingContact.Name = updatedContact.Name;
        existingContact.Email = updatedContact.Email;
        existingContact.Phone = updatedContact.Phone;
        existingContact.IsFavorite = updatedContact.IsFavorite;

        await _context.SaveChangesAsync();
        return existingContact;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
        {
            return false;
        }

        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Contact?> ToggleFavoriteAsync(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
        {
            return null;
        }

        contact.IsFavorite = !contact.IsFavorite;
        await _context.SaveChangesAsync();
        return contact;
    }
}