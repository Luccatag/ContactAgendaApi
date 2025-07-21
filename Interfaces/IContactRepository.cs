using System.Collections.Generic;
using System.Threading.Tasks;
using ContactAgendaApi.Models;

namespace ContactAgendaApi.Interfaces
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAllAsync();
        Task<Contact?> GetByIdAsync(int id);
        Task<Contact> CreateAsync(Contact contact);
        Task<Contact?> UpdateAsync(Contact updatedContact);
        Task<bool> DeleteAsync(int id);
        Task<Contact?> ToggleFavoriteAsync(int id);
    }
}
