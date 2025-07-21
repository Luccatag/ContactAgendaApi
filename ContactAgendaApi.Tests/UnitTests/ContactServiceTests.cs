using ContactAgendaApi.Interfaces;
using ContactAgendaApi.Models;
using ContactAgendaApi.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace ContactAgendaApi.Tests.UnitTests;

/// <summary>
/// Unit tests for ContactService class - the business logic layer of the application.
/// 
/// These tests verify that the ContactService correctly handles:
/// - CRUD operations (Create, Read, Update, Delete)
/// - Business rules and data transformations
/// - Error handling and edge cases
/// - Favorite contact functionality
/// 
/// Uses Moq to mock the IContactRepository dependency, ensuring tests run in isolation
/// and don't depend on actual database operations.
/// </summary>
public class ContactServiceTests
{
    private readonly Mock<IContactRepository> _repositoryMock;
    private readonly ContactService _service;

    /// <summary>
    /// Test setup - Creates mock repository and service instance for each test.
    /// This ensures each test starts with a clean state.
    /// </summary>
    public ContactServiceTests()
    {
        _repositoryMock = new Mock<IContactRepository>();
        _service = new ContactService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllContacts()
    {
        // Arrange
        var contacts = new List<Contact>
        {
            new() { Id = 1, Name = "John Doe", Email = "john@test.com", Phone = "123456789", IsFavorite = false },
            new() { Id = 2, Name = "Jane Smith", Email = "jane@test.com", Phone = "987654321", IsFavorite = true }
        };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(contacts);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(contacts);
        result.Should().HaveCount(2);
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenContactExists_ShouldReturnContact()
    {
        // Arrange
        var contact = new Contact { Id = 1, Name = "John Doe", Email = "john@test.com", Phone = "123456789", IsFavorite = false };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(contact);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().BeEquivalentTo(contact);
        _repositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenContactDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Contact?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
        _repositoryMock.Verify(r => r.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedContact()
    {
        // Arrange
        var newContact = new Contact { Id = 0, Name = "New Contact", Email = "new@test.com", Phone = "555666777", IsFavorite = true };
        var createdContact = new Contact { Id = 3, Name = "New Contact", Email = "new@test.com", Phone = "555666777", IsFavorite = true };

        _repositoryMock.Setup(r => r.CreateAsync(newContact)).ReturnsAsync(createdContact);

        // Act
        var result = await _service.CreateAsync(newContact);

        // Assert
        result.Should().BeEquivalentTo(createdContact);
        result.Id.Should().Be(3); // Verify ID was assigned
        _repositoryMock.Verify(r => r.CreateAsync(newContact), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedContact()
    {
        // Arrange
        var updatedContact = new Contact { Id = 1, Name = "Updated Name", Email = "updated@test.com", Phone = "999888777", IsFavorite = false };
        _repositoryMock.Setup(r => r.UpdateAsync(updatedContact)).ReturnsAsync(updatedContact);

        // Act
        var result = await _service.UpdateAsync(updatedContact);

        // Assert
        result.Should().BeEquivalentTo(updatedContact);
        _repositoryMock.Verify(r => r.UpdateAsync(updatedContact), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenContactDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var nonExistentContact = new Contact { Id = 999, Name = "Non-existent", Email = "none@test.com", Phone = "000000000", IsFavorite = false };
        _repositoryMock.Setup(r => r.UpdateAsync(nonExistentContact)).ReturnsAsync((Contact?)null);

        // Act
        var result = await _service.UpdateAsync(nonExistentContact);

        // Assert
        result.Should().BeNull();
        _repositoryMock.Verify(r => r.UpdateAsync(nonExistentContact), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenContactExists_ShouldReturnTrue()
    {
        // Arrange
        _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
        _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenContactDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        _repositoryMock.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        result.Should().BeFalse();
        _repositoryMock.Verify(r => r.DeleteAsync(999), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_WhenNoContacts_ShouldReturnEmptyCollection()
    {
        // Arrange
        var emptyContacts = new List<Contact>();
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(emptyContacts);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithFavoriteContact_ShouldPreserveFavoriteStatus()
    {
        // Arrange
        var favoriteContact = new Contact { Id = 0, Name = "Favorite Contact", Email = "fav@test.com", Phone = "111222333", IsFavorite = true };
        var createdFavoriteContact = new Contact { Id = 5, Name = "Favorite Contact", Email = "fav@test.com", Phone = "111222333", IsFavorite = true };

        _repositoryMock.Setup(r => r.CreateAsync(favoriteContact)).ReturnsAsync(createdFavoriteContact);

        // Act
        var result = await _service.CreateAsync(favoriteContact);

        // Assert
        result.Should().BeEquivalentTo(createdFavoriteContact);
        result.IsFavorite.Should().BeTrue();
        _repositoryMock.Verify(r => r.CreateAsync(favoriteContact), Times.Once);
    }
}
