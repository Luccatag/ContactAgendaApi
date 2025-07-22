/**
 * Simplified unit tests for ContactForm component
 * 
 * ContactForm is only for creating new contacts, not editing existing ones
 */

import { describe, it, expect, vi, beforeEach } from 'vitest'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import ContactForm from '../../components/ui/ContactForm.vue'

// Mock the contact store with correct method names
const mockStore = {
  addContact: vi.fn(),
  contactExistsByEmail: vi.fn(),
  error: null as string | null
}

vi.mock('../../stores/contactStore', () => ({
  useContactStore: () => mockStore
}))

describe('ContactForm - Fixed', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    vi.clearAllMocks()
    mockStore.error = null
    mockStore.contactExistsByEmail.mockReturnValue(false)
  })

  describe('Form Rendering', () => {
    it('should render all form fields correctly', () => {
      const wrapper = mount(ContactForm)
      
      expect(wrapper.find('#name').exists()).toBe(true)
      expect(wrapper.find('#email').exists()).toBe(true)
      expect(wrapper.find('#phone').exists()).toBe(true)
      expect(wrapper.find('#favorite').exists()).toBe(true)
      expect(wrapper.find('button[type="submit"]').exists()).toBe(true)
      expect(wrapper.find('button[type="button"]').exists()).toBe(true)
    })

    it('should initialize with empty form', () => {
      const wrapper = mount(ContactForm)
      
      expect((wrapper.find('#name').element as HTMLInputElement).value).toBe('')
      expect((wrapper.find('#email').element as HTMLInputElement).value).toBe('')
      expect((wrapper.find('#phone').element as HTMLInputElement).value).toBe('')
      expect((wrapper.find('#favorite').element as HTMLInputElement).checked).toBe(false)
    })

    it('should have proper form structure with title', () => {
      const wrapper = mount(ContactForm, {
        props: { title: 'Test Form' }
      })

      expect(wrapper.find('h2.form-title').text()).toBe('Test Form')
      expect((wrapper.find('#name').element as HTMLInputElement).value).toBe('')
      expect((wrapper.find('#email').element as HTMLInputElement).value).toBe('')
      expect((wrapper.find('#phone').element as HTMLInputElement).value).toBe('')
      expect((wrapper.find('#favorite').element as HTMLInputElement).checked).toBe(false)
    })
  })

  describe('Form Input Handling', () => {
    it('should update form data when inputs change', async () => {
      const wrapper = mount(ContactForm)
      
      await wrapper.find('#name').setValue('Test Name')
      await wrapper.find('#email').setValue('test@example.com')
      await wrapper.find('#phone').setValue('1234567890')
      
      expect((wrapper.find('#name').element as HTMLInputElement).value).toBe('Test Name')
      expect((wrapper.find('#email').element as HTMLInputElement).value).toBe('test@example.com')
      expect((wrapper.find('#phone').element as HTMLInputElement).value).toBe('1234567890')
    })

    it('should handle form reset correctly', async () => {
      const wrapper = mount(ContactForm)
      
      // Fill form first
      await wrapper.find('#name').setValue('Test Name')
      await wrapper.find('#email').setValue('test@example.com')
      
      // Reset form
      await wrapper.find('button[type="button"]').trigger('click')
      
      expect((wrapper.find('#name').element as HTMLInputElement).value).toBe('')
      expect((wrapper.find('#email').element as HTMLInputElement).value).toBe('')
      expect((wrapper.find('#phone').element as HTMLInputElement).value).toBe('')
    })
  })

  describe('Form Validation', () => {
    it('should show validation errors for invalid input', async () => {
      const wrapper = mount(ContactForm)

      // Try to submit form with invalid email
      await wrapper.find('#email').setValue('invalid-email')
      await wrapper.find('#email').trigger('input')
      await wrapper.vm.$nextTick()

      // Check for validation error
      expect(wrapper.find('.validation-error').exists()).toBe(true)
    })

    it('should submit form with valid data for new contact', async () => {
      const wrapper = mount(ContactForm)

      const newContact = {
        id: 1,
        name: 'New Contact',
        email: 'new@example.com',
        phone: '1234567890',
        isFavorite: false
      }
      
      mockStore.addContact.mockResolvedValue(newContact)

      // Fill form with valid data
      await wrapper.find('#name').setValue('New Contact')
      await wrapper.find('#email').setValue('new@example.com')
      await wrapper.find('#phone').setValue('1234567890')
      
      // Ensure form data is updated
      await wrapper.vm.$nextTick()

      // Submit form
      await wrapper.find('form').trigger('submit.prevent')
      await wrapper.vm.$nextTick()

      // Check that addContact was called
      expect(mockStore.addContact).toHaveBeenCalled()
    })

    it('should handle successful contact creation', async () => {
      const wrapper = mount(ContactForm)

      const newContact = {
        id: 1,
        name: 'Success Contact',
        email: 'success@example.com',
        phone: '1234567890',
        isFavorite: false
      }
      
      mockStore.addContact.mockResolvedValue(newContact)

      await wrapper.find('#name').setValue('Success Contact')
      await wrapper.find('#email').setValue('success@example.com')
      await wrapper.find('#phone').setValue('1234567890')

      await wrapper.find('form').trigger('submit.prevent')
      await wrapper.vm.$nextTick()

      expect(wrapper.find('.success-message').exists()).toBe(true)
    })

    it('should clear form after successful submission', async () => {
      const wrapper = mount(ContactForm)

      const newContact = {
        id: 1,
        name: 'Clear Test',
        email: 'clear@example.com',
        phone: '1234567890',
        isFavorite: false
      }
      
      mockStore.addContact.mockResolvedValue(newContact)

      await wrapper.find('#name').setValue('Clear Test')
      await wrapper.find('#email').setValue('clear@example.com')
      await wrapper.find('#phone').setValue('1234567890')

      await wrapper.find('form').trigger('submit.prevent')
      await wrapper.vm.$nextTick()

      // Form should be cleared after successful submission
      expect((wrapper.find('#name').element as HTMLInputElement).value).toBe('')
    })

    it('should toggle favorite checkbox correctly', async () => {
      const wrapper = mount(ContactForm)

      const newContact = {
        id: 1,
        name: 'Favorite Test',
        email: 'favorite@example.com',
        phone: '1234567890',
        isFavorite: true
      }
      
      mockStore.addContact.mockResolvedValue(newContact)

      // Fill form and check favorite
      await wrapper.find('#name').setValue('Favorite Test')
      await wrapper.find('#email').setValue('favorite@example.com')
      await wrapper.find('#phone').setValue('1234567890')
      
      // Check the favorite checkbox
      const favoriteCheckbox = wrapper.find('#favorite')
      await favoriteCheckbox.setValue(true)
      
      // Ensure form data is updated
      await wrapper.vm.$nextTick()

      await wrapper.find('form').trigger('submit.prevent')
      await wrapper.vm.$nextTick()

      // Check that addContact was called
      expect(mockStore.addContact).toHaveBeenCalled()
    })
  })

  describe('Error Handling', () => {
    it('should display error message when contact creation fails', async () => {
      const wrapper = mount(ContactForm)

      mockStore.addContact.mockResolvedValue(null)
      mockStore.error = 'Creation failed'

      await wrapper.find('#name').setValue('Failed Contact')
      await wrapper.find('#email').setValue('failed@example.com')
      await wrapper.find('#phone').setValue('1234567890')
      await wrapper.find('form').trigger('submit.prevent')

      await wrapper.vm.$nextTick()

      expect(wrapper.find('.error-message').exists()).toBe(true)
    })

    it('should handle API errors gracefully', async () => {
      const wrapper = mount(ContactForm)

      mockStore.addContact.mockRejectedValue(new Error('Network error'))

      await wrapper.find('#name').setValue('Network Test')
      await wrapper.find('#email').setValue('network@example.com')
      await wrapper.find('#phone').setValue('1234567890')
      await wrapper.find('form').trigger('submit.prevent')

      await wrapper.vm.$nextTick()

      // Should maintain form state
      expect((wrapper.find('#name').element as HTMLInputElement).value).toBe('Network Test')
    })
  })

  describe('Form State Management', () => {
    it('should show submit button exists', () => {
      const wrapper = mount(ContactForm)
      
      // Form should be rendered
      const submitButton = wrapper.find('button[type="submit"]')
      expect(submitButton.exists()).toBe(true)
    })

    it('should show reset button exists', () => {
      const wrapper = mount(ContactForm)
      
      // Form should be rendered
      const resetButton = wrapper.find('button[type="button"]')
      expect(resetButton.exists()).toBe(true)
    })
  })
})
