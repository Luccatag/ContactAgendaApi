using ContactAgendaApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactAgendaApi.Interfaces;

// Interface for contact service (business logic)
public interface IContactService
{
    Task<IEnumerable<Contact>> GetAllAsync();
    Task<Contact?> GetByIdAsync(int id);
    Task<Contact> CreateAsync(Contact contact);
    Task<Contact?> UpdateAsync(int id, Contact contact);
    Task<bool> DeleteAsync(int id);
}
