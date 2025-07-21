/**
 * Unit tests for ContactCard component
 * 
 * These tests verify that the ContactCard component correctly:
 * - Renders contact information in display mode
 * - Switches to edit mode and renders form fields
 * - Handles user interactions (edit, delete, favorite toggle)
 * - Validates form inputs
 * - Emits correct events to parent components
 */

import { describe, it, expect, vi, beforeEach } from 'vitest'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import ContactCard from '../../components/ui/ContactCard.vue'
import type { Contact } from '../../services/contactService'

// Mock the contact store
const mockStore = {
  updateContact: vi.fn(),
  deleteContact: vi.fn(),
  toggleContactFavorite: vi.fn(),
  error: null
}

vi.mock('../../stores/contactStore', () => ({
  useContactStore: () => mockStore
}))

describe('ContactCard', () => {
  const mockContact: Contact = {
    id: 1,
    name: 'John Doe',
    email: 'john@example.com',
    phone: '1234567890',
    isFavorite: false
  }

  beforeEach(() => {
    setActivePinia(createPinia())
    vi.clearAllMocks()
  })

  describe('Display Mode', () => {
    it('should render contact information correctly', () => {
      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      expect(wrapper.find('.contact-name').text()).toBe('John Doe')
      expect(wrapper.find('.contact-email').text()).toContain('john@example.com')
      expect(wrapper.find('.contact-phone').text()).toContain('1234567890')
    })

    it('should show favorite button with correct state', () => {
      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      const favoriteBtn = wrapper.find('.btn-favorite')
      expect(favoriteBtn.exists()).toBe(true)
      expect(favoriteBtn.text()).toBe('🤍') // Not favorite
      expect(favoriteBtn.classes()).not.toContain('is-favorite')
    })

    it('should show filled heart for favorite contacts', () => {
      const favoriteContact = { ...mockContact, isFavorite: true }
      const wrapper = mount(ContactCard, {
        props: { contact: favoriteContact }
      })

      const favoriteBtn = wrapper.find('.btn-favorite')
      expect(favoriteBtn.text()).toBe('❤️')
      expect(favoriteBtn.classes()).toContain('is-favorite')
    })

    it('should show edit and delete buttons', () => {
      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      expect(wrapper.find('.btn-edit').exists()).toBe(true)
      expect(wrapper.find('.btn-delete').exists()).toBe(true)
    })

    it('should not show edit form initially', () => {
      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      expect(wrapper.find('.contact-edit').exists()).toBe(false)
      expect(wrapper.find('.contact-display').exists()).toBe(true)
    })
  })

  describe('Edit Mode', () => {
    it('should switch to edit mode when edit button is clicked', async () => {
      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      await wrapper.find('.btn-edit').trigger('click')

      expect(wrapper.find('.contact-edit').exists()).toBe(true)
      expect(wrapper.find('.contact-display').exists()).toBe(false)
    })

    it('should populate form fields with current contact data', async () => {
      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      await wrapper.find('.btn-edit').trigger('click')

      const nameInput = wrapper.find('input[type="text"]')
      const emailInput = wrapper.find('input[type="email"]')
      const phoneInput = wrapper.find('input[type="tel"]')
      const favoriteCheckbox = wrapper.find('input[type="checkbox"]')

      expect(nameInput.element.value).toBe('John Doe')
      expect(emailInput.element.value).toBe('john@example.com')
      expect(phoneInput.element.value).toBe('1234567890')
      expect(favoriteCheckbox.element.checked).toBe(false)
    })

    it('should show save and cancel buttons in edit mode', async () => {
      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      await wrapper.find('.btn-edit').trigger('click')

      expect(wrapper.find('.btn-save').exists()).toBe(true)
      expect(wrapper.find('.btn-cancel').exists()).toBe(true)
    })

    it('should return to display mode when cancel is clicked', async () => {
      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      await wrapper.find('.btn-edit').trigger('click')
      await wrapper.find('.btn-cancel').trigger('click')

      expect(wrapper.find('.contact-edit').exists()).toBe(false)
      expect(wrapper.find('.contact-display').exists()).toBe(true)
    })

    it('should validate required fields', async () => {
      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      await wrapper.find('.btn-edit').trigger('click')

      const nameInput = wrapper.find('input[type="text"]')
      await nameInput.setValue('')
      
      const form = wrapper.find('form')
      await form.trigger('submit.prevent')

      // Form should not submit with empty name
      expect(mockStore.updateContact).not.toHaveBeenCalled()
    })
  })

  describe('User Interactions', () => {
    it('should call toggleContactFavorite when favorite button is clicked', async () => {
      mockStore.toggleContactFavorite.mockResolvedValue({
        ...mockContact,
        isFavorite: true
      })

      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      await wrapper.find('.btn-favorite').trigger('click')

      expect(mockStore.toggleContactFavorite).toHaveBeenCalledWith(1)
    })

    it('should call deleteContact when delete button is clicked', async () => {
      // Mock window.confirm to return true
      const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true)
      mockStore.deleteContact.mockResolvedValue(true)

      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      await wrapper.find('.btn-delete').trigger('click')

      expect(confirmSpy).toHaveBeenCalledWith('Are you sure you want to delete this contact?')
      expect(mockStore.deleteContact).toHaveBeenCalledWith(1)

      confirmSpy.mockRestore()
    })

    it('should not delete contact if user cancels confirmation', async () => {
      const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(false)

      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      await wrapper.find('.btn-delete').trigger('click')

      expect(confirmSpy).toHaveBeenCalled()
      expect(mockStore.deleteContact).not.toHaveBeenCalled()

      confirmSpy.mockRestore()
    })

    it('should save changes when form is submitted', async () => {
      const updatedContact = {
        ...mockContact,
        name: 'Jane Doe',
        email: 'jane@example.com'
      }
      mockStore.updateContact.mockResolvedValue(updatedContact)

      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      await wrapper.find('.btn-edit').trigger('click')

      const nameInput = wrapper.find('input[type="text"]')
      const emailInput = wrapper.find('input[type="email"]')

      await nameInput.setValue('Jane Doe')
      await emailInput.setValue('jane@example.com')

      const form = wrapper.find('form')
      await form.trigger('submit.prevent')

      expect(mockStore.updateContact).toHaveBeenCalledWith(1, {
        name: 'Jane Doe',
        email: 'jane@example.com',
        phone: '1234567890',
        isFavorite: false
      })
    })

    it('should return to display mode after successful save', async () => {
      const updatedContact = { ...mockContact, name: 'Updated Name' }
      mockStore.updateContact.mockResolvedValue(updatedContact)

      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      await wrapper.find('.btn-edit').trigger('click')
      
      const nameInput = wrapper.find('input[type="text"]')
      await nameInput.setValue('Updated Name')

      const form = wrapper.find('form')
      await form.trigger('submit.prevent')

      // Wait for async operation
      await wrapper.vm.$nextTick()

      expect(wrapper.find('.contact-edit').exists()).toBe(false)
      expect(wrapper.find('.contact-display').exists()).toBe(true)
    })
  })

  describe('Error Handling', () => {
    it('should handle favorite toggle errors gracefully', async () => {
      mockStore.toggleContactFavorite.mockResolvedValue(null)
      mockStore.error = 'Toggle failed'

      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      await wrapper.find('.btn-favorite').trigger('click')

      // Component should handle the error gracefully
      expect(mockStore.toggleContactFavorite).toHaveBeenCalled()
    })

    it('should handle update errors gracefully', async () => {
      mockStore.updateContact.mockResolvedValue(null)
      mockStore.error = 'Update failed'

      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      await wrapper.find('.btn-edit').trigger('click')
      
      const form = wrapper.find('form')
      await form.trigger('submit.prevent')

      expect(mockStore.updateContact).toHaveBeenCalled()
    })

    it('should handle delete errors gracefully', async () => {
      const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true)
      mockStore.deleteContact.mockResolvedValue(false)
      mockStore.error = 'Delete failed'

      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      await wrapper.find('.btn-delete').trigger('click')

      expect(mockStore.deleteContact).toHaveBeenCalled()

      confirmSpy.mockRestore()
    })
  })

  describe('Props and Validation', () => {
    it('should be required to pass a contact prop', () => {
      // This test ensures the component requires a contact prop
      expect(() => {
        mount(ContactCard, {
          props: {}
        })
      }).toThrow()
    })

    it('should handle contact prop updates', async () => {
      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      const updatedContact = { ...mockContact, name: 'Updated Name' }
      await wrapper.setProps({ contact: updatedContact })

      expect(wrapper.find('.contact-name').text()).toBe('Updated Name')
    })
  })

  describe('Form Validation', () => {
    it('should validate email format', async () => {
      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      await wrapper.find('.btn-edit').trigger('click')

      const emailInput = wrapper.find('input[type="email"]')
      expect(emailInput.attributes('pattern')).toBeDefined()
    })

    it('should validate phone format', async () => {
      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      await wrapper.find('.btn-edit').trigger('click')

      const phoneInput = wrapper.find('input[type="tel"]')
      expect(phoneInput.attributes('pattern')).toBeDefined()
    })

    it('should require name field', async () => {
      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      await wrapper.find('.btn-edit').trigger('click')

      const nameInput = wrapper.find('input[type="text"]')
      expect(nameInput.attributes('required')).toBeDefined()
    })

    it('should require email field', async () => {
      const wrapper = mount(ContactCard, {
        props: { contact: mockContact }
      })

      await wrapper.find('.btn-edit').trigger('click')

      const emailInput = wrapper.find('input[type="email"]')
      expect(emailInput.attributes('required')).toBeDefined()
    })
  })
})
