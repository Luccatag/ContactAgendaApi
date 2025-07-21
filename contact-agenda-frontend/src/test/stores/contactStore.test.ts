/**
 * Unit tests for Contact Store (Pinia)
 * 
 * These tests verify that the contact store correctly:
 * - Manages state and reactive properties
 * - Handles CRUD operations
 * - Provides proper error handling
 * - Implements caching and optimistic updates
 * - Filters and searches contacts correctly
 */

import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { setActivePinia, createPinia } from 'pinia'
import { useContactStore } from '../../stores/contactStore'
import type { Contact, ContactCreateDto } from '../../services/contactService'

// Mock the ContactService
const mockContactService = {
  getAllContacts: vi.fn(),
  getContactById: vi.fn(),
  createContact: vi.fn(),
  updateContact: vi.fn(),
  deleteContact: vi.fn(),
  toggleFavorite: vi.fn()
}

vi.mock('../../services/contactService', () => ({
  ContactService: vi.fn(() => mockContactService)
}))

describe('useContactStore', () => {
  let store: ReturnType<typeof useContactStore>

  beforeEach(() => {
    // Create a fresh Pinia instance for each test
    setActivePinia(createPinia())
    store = useContactStore()
    
    // Clear all mocks
    vi.clearAllMocks()
  })

  afterEach(() => {
    vi.restoreAllMocks()
  })

  describe('Initial State', () => {
    it('should have correct initial state', () => {
      expect(store.contacts).toEqual([])
      expect(store.loading).toBe(false)
      expect(store.error).toBe(null)
      expect(store.selectedContact).toBe(null)
      expect(store.searchQuery).toBe('')
    })
  })

  describe('Computed Properties', () => {
    beforeEach(() => {
      // Set up test data
      store.contacts = [
        { id: 1, name: 'Alice Johnson', email: 'alice@example.com', phone: '1111111111', isFavorite: true },
        { id: 2, name: 'Bob Smith', email: 'bob@example.com', phone: '2222222222', isFavorite: false },
        { id: 3, name: 'Charlie Brown', email: 'charlie@test.com', phone: '3333333333', isFavorite: false }
      ]
    })

    it('should filter contacts by search query', () => {
      store.searchQuery = 'alice'
      expect(store.filteredContacts).toHaveLength(1)
      expect(store.filteredContacts[0].name).toBe('Alice Johnson')
    })

    it('should search across name, email, and phone fields', () => {
      store.searchQuery = 'test.com'
      expect(store.filteredContacts).toHaveLength(1)
      expect(store.filteredContacts[0].email).toBe('charlie@test.com')

      store.searchQuery = '2222'
      expect(store.filteredContacts).toHaveLength(1)
      expect(store.filteredContacts[0].phone).toBe('2222222222')
    })

    it('should be case insensitive when searching', () => {
      store.searchQuery = 'ALICE'
      expect(store.filteredContacts).toHaveLength(1)
      expect(store.filteredContacts[0].name).toBe('Alice Johnson')
    })

    it('should sort favorites first', () => {
      store.searchQuery = ''
      const filtered = store.filteredContacts
      expect(filtered[0].isFavorite).toBe(true)
      expect(filtered[0].name).toBe('Alice Johnson')
    })

    it('should return only favorite contacts when requested', () => {
      const favorites = store.favoriteContacts
      expect(favorites).toHaveLength(1)
      expect(favorites[0].isFavorite).toBe(true)
    })

    it('should return correct contact count', () => {
      expect(store.contactCount).toBe(3)
    })

    it('should identify if contact list is empty', () => {
      expect(store.hasContacts).toBe(true)
      
      store.contacts = []
      expect(store.hasContacts).toBe(false)
    })
  })

  describe('Actions - fetchContacts', () => {
    it('should fetch contacts successfully', async () => {
      // Arrange
      const mockContacts: Contact[] = [
        { id: 1, name: 'John Doe', email: 'john@example.com', phone: '1234567890', isFavorite: false }
      ]
      mockContactService.getAllContacts.mockResolvedValue(mockContacts)

      // Act
      await store.fetchContacts()

      // Assert
      expect(store.loading).toBe(false)
      expect(store.error).toBe(null)
      expect(store.contacts).toEqual(mockContacts)
      expect(mockContactService.getAllContacts).toHaveBeenCalledOnce()
    })

    it('should handle fetch errors gracefully', async () => {
      // Arrange
      const errorMessage = 'Failed to fetch contacts'
      mockContactService.getAllContacts.mockRejectedValue(new Error(errorMessage))

      // Act
      await store.fetchContacts()

      // Assert
      expect(store.loading).toBe(false)
      expect(store.error).toBe(`Failed to load contacts: ${errorMessage}`)
      expect(store.contacts).toEqual([])
    })

    it('should not fetch if already loaded and not forced', async () => {
      // Arrange
      store.contacts = [{ id: 1, name: 'Existing', email: 'existing@example.com', phone: '1111111111', isFavorite: false }]

      // Act
      await store.fetchContacts()

      // Assert
      expect(mockContactService.getAllContacts).not.toHaveBeenCalled()
    })

    it('should fetch even if already loaded when forced', async () => {
      // Arrange
      store.contacts = [{ id: 1, name: 'Existing', email: 'existing@example.com', phone: '1111111111', isFavorite: false }]
      const mockContacts: Contact[] = [
        { id: 2, name: 'New Contact', email: 'new@example.com', phone: '2222222222', isFavorite: false }
      ]
      mockContactService.getAllContacts.mockResolvedValue(mockContacts)

      // Act
      await store.fetchContacts(true)

      // Assert
      expect(mockContactService.getAllContacts).toHaveBeenCalledOnce()
      expect(store.contacts).toEqual(mockContacts)
    })
  })

  describe('Actions - createContact', () => {
    it('should create contact successfully', async () => {
      // Arrange
      const newContactDto: ContactCreateDto = {
        name: 'New Contact',
        email: 'new@example.com',
        phone: '5555555555',
        isFavorite: false
      }
      const createdContact: Contact = { id: 1, ...newContactDto }
      mockContactService.createContact.mockResolvedValue(createdContact)

      // Act
      const result = await store.createContact(newContactDto)

      // Assert
      expect(result).toEqual(createdContact)
      expect(store.contacts).toContain(createdContact)
      expect(store.error).toBe(null)
      expect(mockContactService.createContact).toHaveBeenCalledWith(newContactDto)
    })

    it('should handle creation errors', async () => {
      // Arrange
      const newContactDto: ContactCreateDto = {
        name: '',
        email: 'invalid-email',
        phone: '',
        isFavorite: false
      }
      const errorMessage = 'Validation failed'
      mockContactService.createContact.mockRejectedValue(new Error(errorMessage))

      // Act
      const result = await store.createContact(newContactDto)

      // Assert
      expect(result).toBe(null)
      expect(store.error).toBe(`Failed to create contact: ${errorMessage}`)
      expect(store.contacts).toEqual([])
    })
  })

  describe('Actions - updateContact', () => {
    beforeEach(() => {
      store.contacts = [
        { id: 1, name: 'Original', email: 'original@example.com', phone: '1111111111', isFavorite: false }
      ]
    })

    it('should update contact successfully', async () => {
      // Arrange
      const updateDto: ContactCreateDto = {
        name: 'Updated Name',
        email: 'updated@example.com',
        phone: '9999999999',
        isFavorite: true
      }
      const updatedContact: Contact = { id: 1, ...updateDto }
      mockContactService.updateContact.mockResolvedValue(updatedContact)

      // Act
      const result = await store.updateContact(1, updateDto)

      // Assert
      expect(result).toEqual(updatedContact)
      expect(store.contacts[0]).toEqual(updatedContact)
      expect(store.error).toBe(null)
    })

    it('should handle update errors and revert optimistic update', async () => {
      // Arrange
      const updateDto: ContactCreateDto = {
        name: 'Updated Name',
        email: 'updated@example.com',
        phone: '9999999999'
      }
      const errorMessage = 'Update failed'
      mockContactService.updateContact.mockRejectedValue(new Error(errorMessage))

      // Act
      const result = await store.updateContact(1, updateDto)

      // Assert
      expect(result).toBe(null)
      expect(store.error).toBe(`Failed to update contact: ${errorMessage}`)
      // Contact should be reverted to original state
      expect(store.contacts[0].name).toBe('Original')
    })
  })

  describe('Actions - deleteContact', () => {
    beforeEach(() => {
      store.contacts = [
        { id: 1, name: 'To Delete', email: 'delete@example.com', phone: '1111111111', isFavorite: false },
        { id: 2, name: 'To Keep', email: 'keep@example.com', phone: '2222222222', isFavorite: false }
      ]
    })

    it('should delete contact successfully', async () => {
      // Arrange
      mockContactService.deleteContact.mockResolvedValue(undefined)

      // Act
      const result = await store.deleteContact(1)

      // Assert
      expect(result).toBe(true)
      expect(store.contacts).toHaveLength(1)
      expect(store.contacts[0].id).toBe(2)
      expect(store.error).toBe(null)
    })

    it('should handle delete errors and revert optimistic delete', async () => {
      // Arrange
      const errorMessage = 'Delete failed'
      mockContactService.deleteContact.mockRejectedValue(new Error(errorMessage))

      // Act
      const result = await store.deleteContact(1)

      // Assert
      expect(result).toBe(false)
      expect(store.error).toBe(`Failed to delete contact: ${errorMessage}`)
      // Contact should be restored
      expect(store.contacts).toHaveLength(2)
      expect(store.contacts.find(c => c.id === 1)).toBeDefined()
    })
  })

  describe('Actions - toggleContactFavorite', () => {
    beforeEach(() => {
      store.contacts = [
        { id: 1, name: 'Test Contact', email: 'test@example.com', phone: '1111111111', isFavorite: false }
      ]
    })

    it('should toggle favorite status successfully', async () => {
      // Arrange
      const toggledContact: Contact = { ...store.contacts[0], isFavorite: true }
      mockContactService.toggleFavorite.mockResolvedValue(toggledContact)

      // Act
      const result = await store.toggleContactFavorite(1)

      // Assert
      expect(result).toEqual(toggledContact)
      expect(store.contacts[0].isFavorite).toBe(true)
      expect(store.error).toBe(null)
    })

    it('should handle toggle errors and revert optimistic update', async () => {
      // Arrange
      const errorMessage = 'Toggle failed'
      mockContactService.toggleFavorite.mockRejectedValue(new Error(errorMessage))

      // Act
      const result = await store.toggleContactFavorite(1)

      // Assert
      expect(result).toBe(null)
      expect(store.error).toBe(`Failed to toggle favorite: ${errorMessage}`)
      // Should revert to original state
      expect(store.contacts[0].isFavorite).toBe(false)
    })
  })

  describe('Utility Methods', () => {
    it('should clear error messages', () => {
      store.error = 'Some error'
      store.clearError()
      expect(store.error).toBe(null)
    })

    it('should select a contact', () => {
      const contact: Contact = { id: 1, name: 'Selected', email: 'selected@example.com', phone: '1111111111', isFavorite: false }
      store.selectContact(contact)
      expect(store.selectedContact).toEqual(contact)
    })

    it('should clear selected contact', () => {
      const contact: Contact = { id: 1, name: 'Selected', email: 'selected@example.com', phone: '1111111111', isFavorite: false }
      store.selectContact(contact)
      store.clearSelection()
      expect(store.selectedContact).toBe(null)
    })

    it('should update search query', () => {
      store.setSearchQuery('test query')
      expect(store.searchQuery).toBe('test query')
    })

    it('should find contact by ID', () => {
      const contacts: Contact[] = [
        { id: 1, name: 'Contact 1', email: 'c1@example.com', phone: '1111111111', isFavorite: false },
        { id: 2, name: 'Contact 2', email: 'c2@example.com', phone: '2222222222', isFavorite: false }
      ]
      store.contacts = contacts

      const found = store.findContactById(2)
      expect(found).toEqual(contacts[1])

      const notFound = store.findContactById(999)
      expect(notFound).toBeUndefined()
    })
  })
})
