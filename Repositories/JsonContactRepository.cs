using ContactAgendaApi.Models;
using ContactAgendaApi.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

namespace ContactAgendaApi.Repositories
{
    public class JsonContactRepository : IContactRepository
    {
        private readonly string _filePath;
        public JsonContactRepository(string filePath)
        {
            _filePath = filePath;
        }

        private async Task<List<Contact>> ReadContactsAsync()
        {
            if (!File.Exists(_filePath))
                return new List<Contact>();
            using var stream = File.OpenRead(_filePath);
            return await JsonSerializer.DeserializeAsync<List<Contact>>(stream) ?? new List<Contact>();
        }

        private async Task WriteContactsAsync(List<Contact> contacts)
        {
            using var stream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(stream, contacts);
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await ReadContactsAsync();
        }

        public async Task<Contact?> GetByIdAsync(int id)
        {
            var contacts = await ReadContactsAsync();
            return contacts.FirstOrDefault(c => c.Id == id);
        }

        public async Task<Contact> CreateAsync(Contact contact)
        {
            var contacts = await ReadContactsAsync();
            contact.Id = contacts.Count > 0 ? contacts.Max(c => c.Id) + 1 : 1;
            contacts.Add(contact);
            await WriteContactsAsync(contacts);
            return contact;
        }

        public async Task<Contact?> UpdateAsync(Contact updatedContact)
        {
            var contacts = await ReadContactsAsync();
            var index = contacts.FindIndex(c => c.Id == updatedContact.Id);
            if (index == -1)
                return null;
            contacts[index] = updatedContact;
            await WriteContactsAsync(contacts);
            return updatedContact;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var contacts = await ReadContactsAsync();
            var contact = contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
                return false;
            contacts.Remove(contact);
            await WriteContactsAsync(contacts);
            return true;
        }

        public async Task<Contact?> ToggleFavoriteAsync(int id)
        {
            var contacts = await ReadContactsAsync();
            var contact = contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
                return null;
            
            contact.IsFavorite = !contact.IsFavorite;
            await WriteContactsAsync(contacts);
            return contact;
        }
    }
}
