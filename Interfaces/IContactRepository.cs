using ContactAgendaApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactAgendaApi.Interfaces;

// Interface for contact repository (data access)
public interface IContactRepository
{
    Task<IEnumerable<Contact>> GetAllAsync();
    Task<Contact?> GetByIdAsync(int id);
    Task AddAsync(Contact contact);
    Task UpdateAsync(Contact contact);
    Task DeleteAsync(Contact contact);
    Task<bool> ExistsAsync(int id);
}
