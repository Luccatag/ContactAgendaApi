namespace ContactAgendaApi.DTOs;

// DTO for creating a new contact
public class ContactCreateDto
{
    // The name of the contact (required)
    public string Name { get; set; } = string.Empty;

    // The email address of the contact (required, must be valid)
    public string Email { get; set; } = string.Empty;

    // The phone number of the contact (required, must be valid)
    public string Phone { get; set; } = string.Empty;
}

// DTO for reading (retrieving) a contact
public class ContactReadDto
{
    // The unique identifier of the contact
    public int Id { get; set; }

    // The name of the contact
    public string Name { get; set; } = string.Empty;

    // The email address of the contact
    public string Email { get; set; } = string.Empty;

    // The phone number of the contact
    public string Phone { get; set; } = string.Empty;
}

// DTO for updating an existing contact
public class ContactUpdateDto
{
    // The name of the contact (required)
    public string Name { get; set; } = string.Empty;

    // The email address of the contact (required, must be valid)
    public string Email { get; set; } = string.Empty;

    // The phone number of the contact (required, must be valid)
    public string Phone { get; set; } = string.Empty;
}
