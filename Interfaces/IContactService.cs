using ContactAgendaApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactAgendaApi.Interfaces;

public interface IContactService
{
    Task<IEnumerable<Contact>> GetAllAsync();
    Task<Contact> CreateAsync(Contact contact);
}
