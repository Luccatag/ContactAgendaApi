/**
 * Unit tests for Contact Store (Pinia)
 */

import { describe, it, expect, beforeEach, vi } from 'vitest'
import { setActivePinia, createPinia } from 'pinia'
import { useContactStore } from '../../stores/contactStore'
import type { Contact, ContactCreateDto } from '../../services/contactService'

// Mock the ContactService
vi.mock('../../services/contactService', () => ({
  ContactService: {
    getAllContacts: vi.fn(),
    getContactById: vi.fn(),
    createContact: vi.fn(),
    updateContact: vi.fn(),
    deleteContact: vi.fn(),
    toggleFavorite: vi.fn(),
  }
}))

// Get the mocked service for use in tests
import { ContactService } from '../../services/contactService'
const mockContactService = ContactService as any

describe('useContactStore', () => {
  let store: ReturnType<typeof useContactStore>

  beforeEach(() => {
    setActivePinia(createPinia())
    store = useContactStore()
    
    // Clear all mocks
    vi.clearAllMocks()
  })

  describe('Initial State', () => {
    it('should have correct initial state', () => {
      expect(store.contacts).toEqual([])
      expect(store.loading).toBe(false)
      expect(store.error).toBe(null)
      expect(store.selectedContact).toBe(null)
      expect(store.isInitialized).toBe(false)
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
      const searchResult = store.searchContacts('alice')
      expect(searchResult).toHaveLength(1)
      expect(searchResult[0].name).toBe('Alice Johnson')
    })

    it('should be case insensitive when searching', () => {
      expect(store.searchContacts('ALICE')).toHaveLength(1)
      expect(store.searchContacts('bOb')).toHaveLength(1)
    })

    it('should sort favorites first', () => {
      const sorted = store.sortedContacts
      expect(sorted[0].isFavorite).toBe(true)
      expect(sorted[0].name).toBe('Alice Johnson')
    })

    it('should return correct contact count', () => {
      expect(store.contactCount).toBe(3)
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
      expect(store.error).toBe(errorMessage)
      expect(store.contacts).toEqual([])
    })
  })

  describe('Actions - createContact', () => {
    it('should create contact successfully', async () => {
      // Arrange
      const newContactDto: ContactCreateDto = {
        name: 'New Contact',
        email: 'new@example.com',
        phone: '1234567890',
        isFavorite: false
      }
      const createdContact: Contact = { 
        id: 1, 
        name: newContactDto.name,
        email: newContactDto.email,
        phone: newContactDto.phone,
        isFavorite: newContactDto.isFavorite || false
      }
      mockContactService.createContact.mockResolvedValue(createdContact)

      // Act
      const result = await store.addContact(newContactDto)

      // Assert
      expect(result).toEqual(createdContact)
      expect(store.contacts).toHaveLength(1)
      expect(store.contacts[0]).toEqual(createdContact)
      expect(mockContactService.createContact).toHaveBeenCalledWith(newContactDto)
      expect(store.error).toBe(null)
    })
  })

  describe('Utility Methods', () => {
    it('should select a contact', () => {
      const contact: Contact = { id: 1, name: 'Selected', email: 'selected@example.com', phone: '1111111111', isFavorite: false }
      
      store.setSelectedContact(contact)
      expect(store.selectedContact).toEqual(contact)
    })

    it('should check if contact exists by email', () => {
      store.contacts = [
        { id: 1, name: 'Test', email: 'test@example.com', phone: '1111111111', isFavorite: false }
      ]

      expect(store.contactExistsByEmail('test@example.com')).toBe(true)
      expect(store.contactExistsByEmail('nonexistent@example.com')).toBe(false)
    })
  })
})
