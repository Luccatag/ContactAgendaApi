using ContactAgendaApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactAgendaApi.Interfaces;

public interface IContactService
{
    Task<IEnumerable<Contact>> GetAllAsync();
    Task<Contact?> GetByIdAsync(int id);
    Task<Contact> CreateAsync(Contact contact);
    Task<Contact?> UpdateAsync(Contact contact);
    Task<bool> DeleteAsync(int id);
}
