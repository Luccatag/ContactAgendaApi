using ContactAgendaApi.DTOs;
using ContactAgendaApi.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace ContactAgendaApi.Tests.IntegrationTests;

/// <summary>
/// Integration tests for the ContactsApi - testing complete end-to-end workflows.
/// 
/// These tests verify that the entire API stack works correctly together:
/// - HTTP request/response handling
/// - Database persistence (using in-memory database)
/// - JSON serialization/deserialization
/// - Error handling and status codes
/// - Complete CRUD workflows
/// 
/// Uses WebApplicationFactory to create a test server that hosts the actual API.
/// Uses in-memory database to avoid dependencies on external systems.
/// 
/// Note: Currently failing due to database provider conflicts that need resolution.
/// </summary>
public class ContactsApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    /// <summary>
    /// Test setup - Creates a test server with in-memory database and HTTP client.
    /// 
    /// The WebApplicationFactory creates a real ASP.NET Core server for testing,
    /// but replaces the database with an in-memory version for isolation.
    /// </summary>
    public ContactsApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove existing DbContext registrations
                var descriptors = services.Where(d => 
                    d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                    d.ServiceType == typeof(AppDbContext)).ToList();
                foreach (var descriptor in descriptors)
                {
                    services.Remove(descriptor);
                }

                // Add in-memory database for testing
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Build the service provider and create the database
                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAllContacts_ShouldReturnEmptyListInitially()
    {
        // Act
        var response = await _client.GetAsync("/api/contacts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var contacts = await response.Content.ReadFromJsonAsync<List<ContactReadDto>>();
        contacts.Should().NotBeNull();
        contacts.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateContact_ShouldReturnCreatedContact()
    {
        // Arrange
        var newContact = new ContactCreateDto
        {
            Name = "Integration Test Contact",
            Email = "integration@test.com",
            Phone = "123456789",
            IsFavorite = false
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/contacts", newContact);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdContact = await response.Content.ReadFromJsonAsync<ContactReadDto>();
        createdContact.Should().NotBeNull();
        createdContact!.Name.Should().Be(newContact.Name);
        createdContact.Email.Should().Be(newContact.Email);
        createdContact.Phone.Should().Be(newContact.Phone);
        createdContact.IsFavorite.Should().Be(newContact.IsFavorite);
        createdContact.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetContactById_AfterCreation_ShouldReturnContact()
    {
        // Arrange - Create a contact first
        var newContact = new ContactCreateDto
        {
            Name = "Get By ID Test",
            Email = "getbyid@test.com",
            Phone = "987654321",
            IsFavorite = true
        };

        var createResponse = await _client.PostAsJsonAsync("/api/contacts", newContact);
        var createdContact = await createResponse.Content.ReadFromJsonAsync<ContactReadDto>();

        // Act
        var response = await _client.GetAsync($"/api/contacts/{createdContact!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var retrievedContact = await response.Content.ReadFromJsonAsync<ContactReadDto>();
        retrievedContact.Should().BeEquivalentTo(createdContact);
    }

    [Fact]
    public async Task GetContactById_WhenContactDoesNotExist_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/contacts/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateContact_ShouldReturnUpdatedContact()
    {
        // Arrange - Create a contact first
        var originalContact = new ContactCreateDto
        {
            Name = "Original Name",
            Email = "original@test.com",
            Phone = "111111111",
            IsFavorite = false
        };

        var createResponse = await _client.PostAsJsonAsync("/api/contacts", originalContact);
        var createdContact = await createResponse.Content.ReadFromJsonAsync<ContactReadDto>();

        var updatedContact = new ContactCreateDto
        {
            Name = "Updated Name",
            Email = "updated@test.com",
            Phone = "222222222",
            IsFavorite = true
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/contacts/{createdContact!.Id}", updatedContact);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var resultContact = await response.Content.ReadFromJsonAsync<ContactReadDto>();
        resultContact!.Id.Should().Be(createdContact.Id);
        resultContact.Name.Should().Be(updatedContact.Name);
        resultContact.Email.Should().Be(updatedContact.Email);
        resultContact.Phone.Should().Be(updatedContact.Phone);
        resultContact.IsFavorite.Should().Be(updatedContact.IsFavorite);
    }

    [Fact]
    public async Task UpdateContact_WhenContactDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var updateContact = new ContactCreateDto
        {
            Name = "Non-existent",
            Email = "nonexistent@test.com",
            Phone = "000000000",
            IsFavorite = false
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/contacts/99999", updateContact);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteContact_ShouldRemoveContact()
    {
        // Arrange - Create a contact first
        var newContact = new ContactCreateDto
        {
            Name = "Delete Test Contact",
            Email = "delete@test.com",
            Phone = "333333333",
            IsFavorite = false
        };

        var createResponse = await _client.PostAsJsonAsync("/api/contacts", newContact);
        var createdContact = await createResponse.Content.ReadFromJsonAsync<ContactReadDto>();

        // Act
        var deleteResponse = await _client.DeleteAsync($"/api/contacts/{createdContact!.Id}");

        // Assert - Verify deletion
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify contact no longer exists
        var getResponse = await _client.GetAsync($"/api/contacts/{createdContact.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteContact_WhenContactDoesNotExist_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/api/contacts/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ToggleFavorite_ShouldChangeContactFavoriteStatus()
    {
        // Arrange - Create a contact
        var newContact = new ContactCreateDto
        {
            Name = "Favorite Toggle Test",
            Email = "toggle@test.com",
            Phone = "444444444",
            IsFavorite = false
        };

        var createResponse = await _client.PostAsJsonAsync("/api/contacts", newContact);
        var createdContact = await createResponse.Content.ReadFromJsonAsync<ContactReadDto>();

        // Act - Toggle favorite status
        var toggleResponse = await _client.PatchAsync($"/api/contacts/{createdContact!.Id}/favorite", null);

        // Assert
        toggleResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedContact = await toggleResponse.Content.ReadFromJsonAsync<ContactReadDto>();
        updatedContact!.IsFavorite.Should().BeTrue(); // Should be toggled from false to true

        // Act - Toggle again
        var toggleResponse2 = await _client.PatchAsync($"/api/contacts/{createdContact.Id}/favorite", null);

        // Assert
        toggleResponse2.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedContact2 = await toggleResponse2.Content.ReadFromJsonAsync<ContactReadDto>();
        updatedContact2!.IsFavorite.Should().BeFalse(); // Should be toggled back to false
    }

    [Fact]
    public async Task ToggleFavorite_WhenContactDoesNotExist_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.PatchAsync("/api/contacts/99999/favorite", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateContact_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange - Invalid contact (missing required fields)
        var invalidContact = new ContactCreateDto
        {
            Name = "", // Empty name should be invalid
            Email = "invalid-email", // Invalid email format
            Phone = "",
            IsFavorite = false
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/contacts", invalidContact);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task FullWorkflow_CreateReadUpdateDelete_ShouldWorkCorrectly()
    {
        // Create
        var newContact = new ContactCreateDto
        {
            Name = "Workflow Test Contact",
            Email = "workflow@test.com",
            Phone = "555555555",
            IsFavorite = false
        };

        var createResponse = await _client.PostAsJsonAsync("/api/contacts", newContact);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdContact = await createResponse.Content.ReadFromJsonAsync<ContactReadDto>();

        // Read
        var getResponse = await _client.GetAsync($"/api/contacts/{createdContact!.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var retrievedContact = await getResponse.Content.ReadFromJsonAsync<ContactReadDto>();
        retrievedContact.Should().BeEquivalentTo(createdContact);

        // Update
        var updateData = new ContactCreateDto
        {
            Name = "Updated Workflow Contact",
            Email = "updated-workflow@test.com",
            Phone = "666666666",
            IsFavorite = true
        };

        var updateResponse = await _client.PutAsJsonAsync($"/api/contacts/{createdContact.Id}", updateData);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedContact = await updateResponse.Content.ReadFromJsonAsync<ContactReadDto>();
        updatedContact!.Name.Should().Be(updateData.Name);
        updatedContact.IsFavorite.Should().Be(updateData.IsFavorite);

        // Delete
        var deleteResponse = await _client.DeleteAsync($"/api/contacts/{createdContact.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify deletion
        var verifyResponse = await _client.GetAsync($"/api/contacts/{createdContact.Id}");
        verifyResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
