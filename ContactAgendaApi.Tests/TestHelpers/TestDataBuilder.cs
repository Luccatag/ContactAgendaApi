using ContactAgendaApi.DTOs;
using ContactAgendaApi.Models;

namespace ContactAgendaApi.Tests.TestHelpers;

public static class TestDataBuilder
{
    public static Contact CreateContact(
        int id = 1,
        string name = "Test Contact",
        string email = "test@example.com",
        string phone = "123456789",
        bool isFavorite = false)
    {
        return new Contact
        {
            Id = id,
            Name = name,
            Email = email,
            Phone = phone,
            IsFavorite = isFavorite
        };
    }

    public static ContactCreateDto CreateContactCreateDto(
        string name = "New Contact",
        string email = "new@example.com",
        string phone = "987654321",
        bool isFavorite = false)
    {
        return new ContactCreateDto
        {
            Name = name,
            Email = email,
            Phone = phone,
            IsFavorite = isFavorite
        };
    }

    public static ContactReadDto CreateContactReadDto(
        int id = 1,
        string name = "Read Contact",
        string email = "read@example.com",
        string phone = "555666777",
        bool isFavorite = false)
    {
        return new ContactReadDto
        {
            Id = id,
            Name = name,
            Email = email,
            Phone = phone,
            IsFavorite = isFavorite
        };
    }

    public static ContactUpdateDto CreateContactUpdateDto(
        string name = "Updated Contact",
        string email = "updated@example.com",
        string phone = "111222333",
        bool isFavorite = true)
    {
        return new ContactUpdateDto
        {
            Name = name,
            Email = email,
            Phone = phone,
            IsFavorite = isFavorite
        };
    }

    public static List<Contact> CreateContactList(int count = 3)
    {
        var contacts = new List<Contact>();
        for (int i = 1; i <= count; i++)
        {
            contacts.Add(CreateContact(
                id: i,
                name: $"Contact {i}",
                email: $"contact{i}@example.com",
                phone: $"12345678{i}",
                isFavorite: i % 2 == 0 // Every second contact is favorite
            ));
        }
        return contacts;
    }

    public static List<ContactReadDto> CreateContactReadDtoList(int count = 3)
    {
        var contacts = new List<ContactReadDto>();
        for (int i = 1; i <= count; i++)
        {
            contacts.Add(CreateContactReadDto(
                id: i,
                name: $"Contact {i}",
                email: $"contact{i}@example.com",
                phone: $"12345678{i}",
                isFavorite: i % 2 == 0
            ));
        }
        return contacts;
    }
}
