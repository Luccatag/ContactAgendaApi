using ContactAgendaApi.Controllers;
using ContactAgendaApi.DTOs;
using ContactAgendaApi.Interfaces;
using ContactAgendaApi.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using AutoMapper;

namespace ContactAgendaApi.Tests.UnitTests;

public class ContactsControllerTests
{
    private readonly Mock<IContactService> _serviceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ContactsController _controller;

    public ContactsControllerTests()
    {
        _serviceMock = new Mock<IContactService>();
        _mapperMock = new Mock<IMapper>();
        _controller = new ContactsController(_serviceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkResultWithContactsList()
    {
        // Arrange
        var contacts = new List<Contact>
        {
            new() { Id = 1, Name = "John Doe", Email = "john@test.com", Phone = "123456789", IsFavorite = false },
            new() { Id = 2, Name = "Jane Smith", Email = "jane@test.com", Phone = "987654321", IsFavorite = true }
        };

        var contactDtos = new List<ContactReadDto>
        {
            new() { Id = 1, Name = "John Doe", Email = "john@test.com", Phone = "123456789", IsFavorite = false },
            new() { Id = 2, Name = "Jane Smith", Email = "jane@test.com", Phone = "987654321", IsFavorite = true }
        };

        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(contacts);
        _mapperMock.Setup(m => m.Map<IEnumerable<ContactReadDto>>(contacts)).Returns(contactDtos);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(contactDtos);
        _serviceMock.Verify(s => s.GetAllAsync(), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<ContactReadDto>>(contacts), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenContactExists_ShouldReturnOkResultWithContact()
    {
        // Arrange
        var contact = new Contact { Id = 1, Name = "John Doe", Email = "john@test.com", Phone = "123456789", IsFavorite = false };
        var contactDto = new ContactReadDto { Id = 1, Name = "John Doe", Email = "john@test.com", Phone = "123456789", IsFavorite = false };

        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(contact);
        _mapperMock.Setup(m => m.Map<ContactReadDto>(contact)).Returns(contactDto);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(contactDto);
        _serviceMock.Verify(s => s.GetByIdAsync(1), Times.Once);
        _mapperMock.Verify(m => m.Map<ContactReadDto>(contact), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenContactDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((Contact?)null);

        // Act
        var result = await _controller.GetById(999);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
        _serviceMock.Verify(s => s.GetByIdAsync(999), Times.Once);
        _mapperMock.Verify(m => m.Map<ContactReadDto>(It.IsAny<Contact>()), Times.Never);
    }

    [Fact]
    public async Task Create_WithValidData_ShouldReturnCreatedAtActionResult()
    {
        // Arrange
        var createDto = new ContactCreateDto { Name = "New Contact", Email = "new@test.com", Phone = "555666777", IsFavorite = true };
        var contact = new Contact { Id = 0, Name = "New Contact", Email = "new@test.com", Phone = "555666777", IsFavorite = true };
        var createdContact = new Contact { Id = 3, Name = "New Contact", Email = "new@test.com", Phone = "555666777", IsFavorite = true };
        var contactDto = new ContactReadDto { Id = 3, Name = "New Contact", Email = "new@test.com", Phone = "555666777", IsFavorite = true };

        _mapperMock.Setup(m => m.Map<Contact>(createDto)).Returns(contact);
        _serviceMock.Setup(s => s.CreateAsync(contact)).ReturnsAsync(createdContact);
        _mapperMock.Setup(m => m.Map<ContactReadDto>(createdContact)).Returns(contactDto);

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var createdAtActionResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdAtActionResult.ActionName.Should().Be(nameof(ContactsController.GetById));
        createdAtActionResult.RouteValues!["id"].Should().Be(3);
        createdAtActionResult.Value.Should().BeEquivalentTo(contactDto);

        _mapperMock.Verify(m => m.Map<Contact>(createDto), Times.Once);
        _serviceMock.Verify(s => s.CreateAsync(contact), Times.Once);
        _mapperMock.Verify(m => m.Map<ContactReadDto>(createdContact), Times.Once);
    }

    [Fact]
    public async Task Update_WhenContactExists_ShouldReturnOkResultWithUpdatedContact()
    {
        // Arrange
        var updateDto = new ContactCreateDto { Name = "Updated Name", Email = "updated@test.com", Phone = "999888777", IsFavorite = false };
        var existingContact = new Contact { Id = 1, Name = "Old Name", Email = "old@test.com", Phone = "111222333", IsFavorite = true };
        var updatedContact = new Contact { Id = 1, Name = "Updated Name", Email = "updated@test.com", Phone = "999888777", IsFavorite = false };
        var contactDto = new ContactReadDto { Id = 1, Name = "Updated Name", Email = "updated@test.com", Phone = "999888777", IsFavorite = false };

        _mapperMock.Setup(m => m.Map<Contact>(updateDto)).Returns(updatedContact);
        _serviceMock.Setup(s => s.UpdateAsync(It.Is<Contact>(c => c.Id == 1))).ReturnsAsync(updatedContact);
        _mapperMock.Setup(m => m.Map<ContactReadDto>(updatedContact)).Returns(contactDto);

        // Act
        var result = await _controller.Update(1, updateDto);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(contactDto);

        _mapperMock.Verify(m => m.Map<Contact>(updateDto), Times.Once);
        _serviceMock.Verify(s => s.UpdateAsync(It.Is<Contact>(c => c.Id == 1)), Times.Once);
        _mapperMock.Verify(m => m.Map<ContactReadDto>(updatedContact), Times.Once);
    }

    [Fact]
    public async Task Update_WhenContactDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var updateDto = new ContactCreateDto { Name = "Updated Name", Email = "updated@test.com", Phone = "999888777", IsFavorite = false };
        var mappedContact = new Contact { Id = 999, Name = "Updated Name", Email = "updated@test.com", Phone = "999888777", IsFavorite = false };
        
        _mapperMock.Setup(m => m.Map<Contact>(updateDto)).Returns(mappedContact);
        _serviceMock.Setup(s => s.UpdateAsync(It.Is<Contact>(c => c.Id == 999))).ReturnsAsync((Contact?)null);

        // Act
        var result = await _controller.Update(999, updateDto);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
        _mapperMock.Verify(m => m.Map<Contact>(updateDto), Times.Once);
        _serviceMock.Verify(s => s.UpdateAsync(It.Is<Contact>(c => c.Id == 999)), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenContactExists_ShouldReturnNoContent()
    {
        // Arrange
        _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _serviceMock.Verify(s => s.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenContactDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.DeleteAsync(999)).ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _serviceMock.Verify(s => s.DeleteAsync(999), Times.Once);
    }

    [Fact]
    public async Task ToggleFavorite_WhenContactExists_ShouldReturnOkResultWithUpdatedContact()
    {
        // Arrange
        var existingContact = new Contact { Id = 1, Name = "Test Contact", Email = "test@test.com", Phone = "123456789", IsFavorite = false };
        var updatedContact = new Contact { Id = 1, Name = "Test Contact", Email = "test@test.com", Phone = "123456789", IsFavorite = true };
        var contactDto = new ContactReadDto { Id = 1, Name = "Test Contact", Email = "test@test.com", Phone = "123456789", IsFavorite = true };

        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(existingContact);
        _serviceMock.Setup(s => s.UpdateAsync(It.Is<Contact>(c => c.IsFavorite == true))).ReturnsAsync(updatedContact);
        _mapperMock.Setup(m => m.Map<ContactReadDto>(updatedContact)).Returns(contactDto);

        // Act
        var result = await _controller.ToggleFavorite(1);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(contactDto);
        
        _serviceMock.Verify(s => s.GetByIdAsync(1), Times.Once);
        _serviceMock.Verify(s => s.UpdateAsync(It.Is<Contact>(c => c.IsFavorite == true)), Times.Once);
        _mapperMock.Verify(m => m.Map<ContactReadDto>(updatedContact), Times.Once);
    }

    [Fact]
    public async Task ToggleFavorite_WhenContactDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((Contact?)null);

        // Act
        var result = await _controller.ToggleFavorite(999);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
        _serviceMock.Verify(s => s.GetByIdAsync(999), Times.Once);
        _serviceMock.Verify(s => s.UpdateAsync(It.IsAny<Contact>()), Times.Never);
    }

    [Fact]
    public async Task GetAll_WhenNoContacts_ShouldReturnOkResultWithEmptyList()
    {
        // Arrange
        var emptyContacts = new List<Contact>();
        var emptyDtos = new List<ContactReadDto>();

        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(emptyContacts);
        _mapperMock.Setup(m => m.Map<IEnumerable<ContactReadDto>>(emptyContacts)).Returns(emptyDtos);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedContacts = okResult.Value.Should().BeAssignableTo<IEnumerable<ContactReadDto>>().Subject;
        returnedContacts.Should().BeEmpty();
        _serviceMock.Verify(s => s.GetAllAsync(), Times.Once);
    }
}
