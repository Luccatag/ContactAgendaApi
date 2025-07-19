
// Import DTOs and FluentValidation library
using ContactAgendaApi.DTOs;
using FluentValidation;

namespace ContactAgendaApi.Validators;

// Validator for creating a new contact
public class ContactCreateDtoValidator : AbstractValidator<ContactCreateDto>
{
    public ContactCreateDtoValidator()
    {
        // Name is required and must be at most 100 characters
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100);

        // Email is required and must be a valid email address
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        // Phone is required and must match a simple phone number pattern
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .Matches(@"^\\+?[0-9]{7,15}$").WithMessage("Invalid phone number format.");
    }
}

// Validator for updating an existing contact
public class ContactUpdateDtoValidator : AbstractValidator<ContactUpdateDto>
{
    public ContactUpdateDtoValidator()
    {
        // Name is required and must be at most 100 characters
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100);

        // Email is required and must be a valid email address
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        // Phone is required and must match a simple phone number pattern
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .Matches(@"^\\+?[0-9]{7,15}$").WithMessage("Invalid phone number format.");
    }
}
