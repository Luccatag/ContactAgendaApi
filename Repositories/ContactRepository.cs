using ContactAgendaApi.Models;
using ContactAgendaApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ContactAgendaApi.Repositories;

// Implementation of IContactRepository using EF Core
public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _context;
    public ContactRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Contact>> GetAllAsync()
        => await _context.Contacts.ToListAsync();

    public async Task<Contact?> GetByIdAsync(int id)
        => await _context.Contacts.FindAsync(id);

    public async Task AddAsync(Contact contact)
    {
        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Contact contact)
    {
        _context.Contacts.Update(contact);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Contact contact)
    {
        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
        => await _context.Contacts.AnyAsync(c => c.Id == id);
}
