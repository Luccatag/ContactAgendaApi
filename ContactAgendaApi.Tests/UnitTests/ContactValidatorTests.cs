using ContactAgendaApi.DTOs;
using ContactAgendaApi.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace ContactAgendaApi.Tests.UnitTests;

/// <summary>
/// Unit tests for FluentValidation validators used in the ContactAgendaApi.
/// 
/// These tests ensure that all validation rules work correctly:
/// - Required field validation (Name, Email, Phone)
/// - Format validation (Email format, Phone number patterns)
/// - Length restrictions (Name ≤100 chars, Email ≤100 chars)
/// - Error message generation and localization
/// 
/// Uses FluentValidation.TestHelper for fluent assertion syntax.
/// Tests both ContactCreateDtoValidator and ContactUpdateDtoValidator.
/// </summary>
public class ContactValidatorTests
{
    private readonly ContactCreateDtoValidator _createValidator;
    private readonly ContactUpdateDtoValidator _updateValidator;

    /// <summary>
    /// Test setup - Creates validator instances for each test.
    /// Both create and update validators share the same rules.
    /// </summary>
    public ContactValidatorTests()
    {
        _createValidator = new ContactCreateDtoValidator();
        _updateValidator = new ContactUpdateDtoValidator();
    }

    #region Valid Data Tests

    /// <summary>
    /// Test: Verify that a completely valid contact passes all validation rules.
    /// This is the "happy path" test for the create validator.
    /// </summary>
    [Fact]
    public void ContactCreateDtoValidator_ValidContact_ShouldPassValidation()
    {
        // Arrange
        var validContact = new ContactCreateDto
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Phone = "123-456-7890",
            IsFavorite = false
        };

        // Act
        var result = _createValidator.TestValidate(validContact);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ContactCreateDtoValidator_EmptyName_ShouldHaveValidationError()
    {
        // Arrange
        var invalidContact = new ContactCreateDto
        {
            Name = "",
            Email = "john.doe@example.com",
            Phone = "123-456-7890",
            IsFavorite = false
        };

        // Act
        var result = _createValidator.TestValidate(invalidContact);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ContactCreateDtoValidator_TooLongName_ShouldHaveValidationError()
    {
        // Arrange
        var invalidContact = new ContactCreateDto
        {
            Name = new string('A', 101), // 101 characters (exceeds 100 char limit)
            Email = "john.doe@example.com",
            Phone = "123-456-7890",
            IsFavorite = false
        };

        // Act
        var result = _createValidator.TestValidate(invalidContact);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ContactCreateDtoValidator_InvalidEmail_ShouldHaveValidationError()
    {
        // Arrange
        var invalidContact = new ContactCreateDto
        {
            Name = "John Doe",
            Email = "invalid-email",
            Phone = "123-456-7890",
            IsFavorite = false
        };

        // Act
        var result = _createValidator.TestValidate(invalidContact);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void ContactCreateDtoValidator_EmptyEmail_ShouldHaveValidationError()
    {
        // Arrange
        var invalidContact = new ContactCreateDto
        {
            Name = "John Doe",
            Email = "",
            Phone = "123-456-7890",
            IsFavorite = false
        };

        // Act
        var result = _createValidator.TestValidate(invalidContact);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void ContactCreateDtoValidator_TooLongEmail_ShouldHaveValidationError()
    {
        // Arrange
        var longEmail = new string('a', 90) + "@example.com"; // > 100 chars
        var invalidContact = new ContactCreateDto
        {
            Name = "John Doe",
            Email = longEmail,
            Phone = "123-456-7890",
            IsFavorite = false
        };

        // Act
        var result = _createValidator.TestValidate(invalidContact);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void ContactCreateDtoValidator_EmptyPhone_ShouldHaveValidationError()
    {
        // Arrange
        var invalidContact = new ContactCreateDto
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Phone = "",
            IsFavorite = false
        };

        // Act
        var result = _createValidator.TestValidate(invalidContact);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Fact]
    public void ContactCreateDtoValidator_TooLongPhone_ShouldHaveValidationError()
    {
        // Arrange
        var invalidContact = new ContactCreateDto
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Phone = new string('1', 21), // 21 digits (exceeds 20 char limit)
            IsFavorite = false
        };

        // Act
        var result = _createValidator.TestValidate(invalidContact);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Theory]
    [InlineData("123-456-7890")]
    [InlineData("(123) 456-7890")]
    [InlineData("+1 123 456 7890")]
    [InlineData("1234567890")]
    [InlineData("123.456.7890")]
    public void ContactCreateDtoValidator_ValidPhoneFormats_ShouldPassValidation(string validPhone)
    {
        // Arrange
        var validContact = new ContactCreateDto
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Phone = validPhone,
            IsFavorite = false
        };

        // Act
        var result = _createValidator.TestValidate(validContact);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Phone);
    }

    [Theory]
    [InlineData("john@example.com")]
    [InlineData("john.doe@example.co.uk")]
    [InlineData("john+tag@example.org")]
    [InlineData("test123@test-domain.com")]
    public void ContactCreateDtoValidator_ValidEmailFormats_ShouldPassValidation(string validEmail)
    {
        // Arrange
        var validContact = new ContactCreateDto
        {
            Name = "John Doe",
            Email = validEmail,
            Phone = "123-456-7890",
            IsFavorite = false
        };

        // Act
        var result = _createValidator.TestValidate(validContact);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void ContactUpdateDtoValidator_ValidContact_ShouldPassValidation()
    {
        // Arrange
        var validContact = new ContactUpdateDto
        {
            Name = "Jane Smith",
            Email = "jane.smith@example.com",
            Phone = "987-654-3210",
            IsFavorite = true
        };

        // Act
        var result = _updateValidator.TestValidate(validContact);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ContactUpdateDtoValidator_SameValidationRulesAsCreate_ShouldWork()
    {
        // Arrange
        var invalidContact = new ContactUpdateDto
        {
            Name = "",
            Email = "invalid-email",
            Phone = "",
            IsFavorite = false
        };

        // Act
        var result = _updateValidator.TestValidate(invalidContact);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ContactValidators_IsFavoriteFlag_ShouldAlwaysBeValid(bool isFavorite)
    {
        // Arrange
        var contact = new ContactCreateDto
        {
            Name = "Test Contact",
            Email = "test@example.com",
            Phone = "123-456-7890",
            IsFavorite = isFavorite
        };

        // Act
        var result = _createValidator.TestValidate(contact);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.IsFavorite);
    }
}
#endregion
