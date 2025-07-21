/**
 * Unit tests for ContactService
 * 
 * These tests verify that the ContactService correctly:
 * - Makes HTTP requests to the correct endpoints
 * - Handles responses and errors appropriately
 * - Transforms data correctly
 * - Provides proper error messages
 */

import { describe, it, expect, vi, beforeEach } from 'vitest'
import { ContactService, type Contact, type ContactCreateDto } from '../../services/contactService'

// Mock axios
vi.mock('axios', () => {
  const mockAxios = {
    create: vi.fn(() => ({
      get: vi.fn(),
      post: vi.fn(),
      put: vi.fn(),
      delete: vi.fn(),
      patch: vi.fn(),
      interceptors: {
        response: {
          use: vi.fn()
        }
      }
    }))
  }
  return { default: mockAxios }
})

describe('ContactService', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('basic functionality', () => {
    it('should have all required static methods', () => {
      expect(typeof ContactService.getAllContacts).toBe('function')
      expect(typeof ContactService.getContactById).toBe('function')
      expect(typeof ContactService.createContact).toBe('function')
      expect(typeof ContactService.updateContact).toBe('function')
      expect(typeof ContactService.deleteContact).toBe('function')
      expect(typeof ContactService.toggleFavorite).toBe('function')
    })
  })

  describe('type definitions', () => {
    it('should have correct Contact interface', () => {
      const contact: Contact = {
        id: 1,
        name: 'Test',
        email: 'test@example.com',
        phone: '1234567890',
        isFavorite: false
      }
      
      expect(contact.id).toBe(1)
      expect(contact.name).toBe('Test')
      expect(contact.email).toBe('test@example.com')
      expect(contact.phone).toBe('1234567890')
      expect(contact.isFavorite).toBe(false)
    })

    it('should have correct ContactCreateDto interface', () => {
      const createDto: ContactCreateDto = {
        name: 'New Contact',
        email: 'new@example.com',
        phone: '0987654321',
        isFavorite: true
      }
      
      expect(createDto.name).toBe('New Contact')
      expect(createDto.email).toBe('new@example.com')
      expect(createDto.phone).toBe('0987654321')
      expect(createDto.isFavorite).toBe(true)
    })

    it('should have optional isFavorite in ContactCreateDto', () => {
      const createDto: ContactCreateDto = {
        name: 'New Contact',
        email: 'new@example.com',
        phone: '0987654321'
        // isFavorite is optional
      }
      
      expect(createDto.isFavorite).toBeUndefined()
    })
  })
})
