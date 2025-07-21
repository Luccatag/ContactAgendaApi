/**
 * Unit tests for ContactForm component
 * 
 * These tests verify that the ContactForm component correctly:
 * - Renders form fields and labels
 * - Handles form validation (client-side)
 * - Manages form state and data binding
 * - Emits correct events on form submission
 * - Shows validation errors appropriately
 * - Handles loading states during submission
 */

import { describe, it, expect, vi, beforeEach } from 'vitest'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import ContactForm from '../../components/ui/ContactForm.vue'
import type { Contact, ContactCreateDto } from '../../services/contactService'

// Mock the contact store
const mockStore = {
  createContact: vi.fn(),
  error: null,
  loading: false
}

vi.mock('../../stores/contactStore', () => ({
  useContactStore: () => mockStore
}))

describe('ContactForm', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    vi.clearAllMocks()
  })

  describe('Form Rendering', () => {
    it('should render all form fields', () => {
      const wrapper = mount(ContactForm)

      expect(wrapper.find('#name').exists()).toBe(true)
      expect(wrapper.find('#email').exists()).toBe(true)
      expect(wrapper.find('#phone').exists()).toBe(true)
      expect(wrapper.find('#favorite').exists()).toBe(true)
      expect(wrapper.find('button[type="submit"]').exists()).toBe(true)
    })

    it('should render optional title when provided', () => {
      const wrapper = mount(ContactForm, {
        props: { title: 'Add New Contact' }
      })

      expect(wrapper.find('.form-title').text()).toBe('Add New Contact')
    })

    it('should not render title when not provided', () => {
      const wrapper = mount(ContactForm)

      expect(wrapper.find('.form-title').exists()).toBe(false)
    })

    it('should render proper labels for accessibility', () => {
      const wrapper = mount(ContactForm)

      const nameLabel = wrapper.find('label[for="name"]')
      const emailLabel = wrapper.find('label[for="email"]')
      const phoneLabel = wrapper.find('label[for="phone"]')
      const favoriteLabel = wrapper.find('label[for="favorite"]')

      expect(nameLabel.exists()).toBe(true)
      expect(emailLabel.exists()).toBe(true)
      expect(phoneLabel.exists()).toBe(true)
      expect(favoriteLabel.exists()).toBe(true)
    })

    it('should have proper input attributes for validation', () => {
      const wrapper = mount(ContactForm)

      const nameInput = wrapper.find('#name')
      const emailInput = wrapper.find('#email')
      const phoneInput = wrapper.find('#phone')

      expect(nameInput.attributes('required')).toBeDefined()
      expect(emailInput.attributes('required')).toBeDefined()
      expect(emailInput.attributes('type')).toBe('email')
      expect(emailInput.attributes('pattern')).toBeDefined()
      expect(phoneInput.attributes('required')).toBeDefined()
      expect(phoneInput.attributes('type')).toBe('tel')
      expect(phoneInput.attributes('pattern')).toBeDefined()
    })
  })

  describe('Form State Management', () => {
    it('should initialize with empty form data', () => {
      const wrapper = mount(ContactForm)

      expect(wrapper.find('#name').element.value).toBe('')
      expect(wrapper.find('#email').element.value).toBe('')
      expect(wrapper.find('#phone').element.value).toBe('')
      expect(wrapper.find('#favorite').element.checked).toBe(false)
    })

    it('should populate form with initial data when provided', () => {
      const initialData: Contact = {
        id: 1,
        name: 'John Doe',
        email: 'john@example.com',
        phone: '1234567890',
        isFavorite: true
      }

      const wrapper = mount(ContactForm, {
        props: { initialData }
      })

      expect(wrapper.find('#name').element.value).toBe('John Doe')
      expect(wrapper.find('#email').element.value).toBe('john@example.com')
      expect(wrapper.find('#phone').element.value).toBe('1234567890')
      expect(wrapper.find('#favorite').element.checked).toBe(true)
    })

    it('should update form data when user types', async () => {
      const wrapper = mount(ContactForm)

      const nameInput = wrapper.find('#name')
      const emailInput = wrapper.find('#email')
      const phoneInput = wrapper.find('#phone')

      await nameInput.setValue('Jane Doe')
      await emailInput.setValue('jane@example.com')
      await phoneInput.setValue('0987654321')

      expect(nameInput.element.value).toBe('Jane Doe')
      expect(emailInput.element.value).toBe('jane@example.com')
      expect(phoneInput.element.value).toBe('0987654321')
    })

    it('should toggle favorite checkbox', async () => {
      const wrapper = mount(ContactForm)

      const favoriteInput = wrapper.find('#favorite')
      
      expect(favoriteInput.element.checked).toBe(false)
      
      await favoriteInput.setChecked(true)
      
      expect(favoriteInput.element.checked).toBe(true)
    })
  })

  describe('Form Validation', () => {
    it('should show email validation error for invalid email', async () => {
      const wrapper = mount(ContactForm)

      const emailInput = wrapper.find('#email')
      await emailInput.setValue('invalid-email')
      await emailInput.trigger('input')

      await wrapper.vm.$nextTick()

      const errorMessage = wrapper.find('.validation-error')
      expect(errorMessage.exists()).toBe(true)
      expect(errorMessage.text()).toContain('valid email')
    })

    it('should clear email validation error for valid email', async () => {
      const wrapper = mount(ContactForm)

      const emailInput = wrapper.find('#email')
      
      // First set invalid email
      await emailInput.setValue('invalid-email')
      await emailInput.trigger('input')
      await wrapper.vm.$nextTick()
      
      expect(wrapper.find('.validation-error').exists()).toBe(true)

      // Then set valid email
      await emailInput.setValue('valid@example.com')
      await emailInput.trigger('input')
      await wrapper.vm.$nextTick()

      expect(wrapper.find('.validation-error').exists()).toBe(false)
    })

    it('should show phone validation error for invalid phone', async () => {
      const wrapper = mount(ContactForm)

      const phoneInput = wrapper.find('#phone')
      await phoneInput.setValue('123') // Too short
      await phoneInput.trigger('input')

      await wrapper.vm.$nextTick()

      const errorMessage = wrapper.find('.validation-error')
      expect(errorMessage.exists()).toBe(true)
      expect(errorMessage.text()).toContain('phone')
    })

    it('should clear phone validation error for valid phone', async () => {
      const wrapper = mount(ContactForm)

      const phoneInput = wrapper.find('#phone')
      
      // First set invalid phone
      await phoneInput.setValue('123')
      await phoneInput.trigger('input')
      await wrapper.vm.$nextTick()
      
      expect(wrapper.find('.validation-error').exists()).toBe(true)

      // Then set valid phone
      await phoneInput.setValue('1234567890')
      await phoneInput.trigger('input')
      await wrapper.vm.$nextTick()

      expect(wrapper.find('.validation-error').exists()).toBe(false)
    })

    it('should not submit form with invalid data', async () => {
      const wrapper = mount(ContactForm)

      // Set invalid data
      await wrapper.find('#name').setValue('')
      await wrapper.find('#email').setValue('invalid-email')
      await wrapper.find('#phone').setValue('123')

      const form = wrapper.find('form')
      await form.trigger('submit.prevent')

      // Should not call createContact with invalid data
      expect(mockStore.createContact).not.toHaveBeenCalled()
    })
  })

  describe('Form Submission', () => {
    it('should submit form with valid data', async () => {
      const newContact = {
        id: 1,
        name: 'John Doe',
        email: 'john@example.com',
        phone: '1234567890',
        isFavorite: false
      }
      mockStore.createContact.mockResolvedValue(newContact)

      const wrapper = mount(ContactForm)

      // Fill form with valid data
      await wrapper.find('#name').setValue('John Doe')
      await wrapper.find('#email').setValue('john@example.com')
      await wrapper.find('#phone').setValue('1234567890')

      const form = wrapper.find('form')
      await form.trigger('submit.prevent')

      expect(mockStore.createContact).toHaveBeenCalledWith({
        name: 'John Doe',
        email: 'john@example.com',
        phone: '1234567890',
        isFavorite: false
      })
    })

    it('should emit success event after successful submission', async () => {
      const newContact = {
        id: 1,
        name: 'John Doe',
        email: 'john@example.com',
        phone: '1234567890',
        isFavorite: false
      }
      mockStore.createContact.mockResolvedValue(newContact)

      const wrapper = mount(ContactForm)

      // Fill and submit form
      await wrapper.find('#name').setValue('John Doe')
      await wrapper.find('#email').setValue('john@example.com')
      await wrapper.find('#phone').setValue('1234567890')

      const form = wrapper.find('form')
      await form.trigger('submit.prevent')

      // Wait for async operation
      await wrapper.vm.$nextTick()

      expect(wrapper.emitted('success')).toBeTruthy()
      expect(wrapper.emitted('success')[0]).toEqual([newContact])
    })

    it('should clear form after successful submission', async () => {
      const newContact = {
        id: 1,
        name: 'John Doe',
        email: 'john@example.com',
        phone: '1234567890',
        isFavorite: false
      }
      mockStore.createContact.mockResolvedValue(newContact)

      const wrapper = mount(ContactForm)

      // Fill and submit form
      await wrapper.find('#name').setValue('John Doe')
      await wrapper.find('#email').setValue('john@example.com')
      await wrapper.find('#phone').setValue('1234567890')

      const form = wrapper.find('form')
      await form.trigger('submit.prevent')

      // Wait for async operation
      await wrapper.vm.$nextTick()

      // Form should be cleared
      expect(wrapper.find('#name').element.value).toBe('')
      expect(wrapper.find('#email').element.value).toBe('')
      expect(wrapper.find('#phone').element.value).toBe('')
      expect(wrapper.find('#favorite').element.checked).toBe(false)
    })

    it('should include favorite status in submission', async () => {
      const newContact = {
        id: 1,
        name: 'John Doe',
        email: 'john@example.com',
        phone: '1234567890',
        isFavorite: true
      }
      mockStore.createContact.mockResolvedValue(newContact)

      const wrapper = mount(ContactForm)

      // Fill form with favorite checked
      await wrapper.find('#name').setValue('John Doe')
      await wrapper.find('#email').setValue('john@example.com')
      await wrapper.find('#phone').setValue('1234567890')
      await wrapper.find('#favorite').setChecked(true)

      const form = wrapper.find('form')
      await form.trigger('submit.prevent')

      expect(mockStore.createContact).toHaveBeenCalledWith({
        name: 'John Doe',
        email: 'john@example.com',
        phone: '1234567890',
        isFavorite: true
      })
    })
  })

  describe('Error Handling', () => {
    it('should handle submission errors gracefully', async () => {
      mockStore.createContact.mockResolvedValue(null)
      mockStore.error = 'Creation failed'

      const wrapper = mount(ContactForm)

      // Fill and submit form
      await wrapper.find('#name').setValue('John Doe')
      await wrapper.find('#email').setValue('john@example.com')
      await wrapper.find('#phone').setValue('1234567890')

      const form = wrapper.find('form')
      await form.trigger('submit.prevent')

      expect(mockStore.createContact).toHaveBeenCalled()
      // Form should not be cleared on error
      expect(wrapper.find('#name').element.value).toBe('John Doe')
    })

    it('should not emit success event on submission failure', async () => {
      mockStore.createContact.mockResolvedValue(null)

      const wrapper = mount(ContactForm)

      // Fill and submit form
      await wrapper.find('#name').setValue('John Doe')
      await wrapper.find('#email').setValue('john@example.com')
      await wrapper.find('#phone').setValue('1234567890')

      const form = wrapper.find('form')
      await form.trigger('submit.prevent')

      await wrapper.vm.$nextTick()

      expect(wrapper.emitted('success')).toBeFalsy()
    })
  })

  describe('Loading States', () => {
    it('should disable submit button when loading', async () => {
      mockStore.loading = true

      const wrapper = mount(ContactForm)

      const submitButton = wrapper.find('button[type="submit"]')
      expect(submitButton.attributes('disabled')).toBeDefined()
    })

    it('should enable submit button when not loading', async () => {
      mockStore.loading = false

      const wrapper = mount(ContactForm)

      const submitButton = wrapper.find('button[type="submit"]')
      expect(submitButton.attributes('disabled')).toBeUndefined()
    })

    it('should show loading text on submit button when loading', () => {
      mockStore.loading = true

      const wrapper = mount(ContactForm)

      const submitButton = wrapper.find('button[type="submit"]')
      expect(submitButton.text()).toContain('Adding')
    })

    it('should show normal text on submit button when not loading', () => {
      mockStore.loading = false

      const wrapper = mount(ContactForm)

      const submitButton = wrapper.find('button[type="submit"]')
      expect(submitButton.text()).toBe('Add Contact')
    })
  })

  describe('Props and Events', () => {
    it('should accept and use title prop', () => {
      const wrapper = mount(ContactForm, {
        props: { title: 'Create New Contact' }
      })

      expect(wrapper.find('.form-title').text()).toBe('Create New Contact')
    })

    it('should accept and use initialData prop', () => {
      const initialData: Contact = {
        id: 1,
        name: 'Test Contact',
        email: 'test@example.com',
        phone: '1111111111',
        isFavorite: true
      }

      const wrapper = mount(ContactForm, {
        props: { initialData }
      })

      expect(wrapper.find('#name').element.value).toBe('Test Contact')
      expect(wrapper.find('#email').element.value).toBe('test@example.com')
      expect(wrapper.find('#phone').element.value).toBe('1111111111')
      expect(wrapper.find('#favorite').element.checked).toBe(true)
    })

    it('should handle initialData updates', async () => {
      const wrapper = mount(ContactForm)

      const newInitialData: Contact = {
        id: 2,
        name: 'Updated Contact',
        email: 'updated@example.com',
        phone: '2222222222',
        isFavorite: false
      }

      await wrapper.setProps({ initialData: newInitialData })

      expect(wrapper.find('#name').element.value).toBe('Updated Contact')
      expect(wrapper.find('#email').element.value).toBe('updated@example.com')
      expect(wrapper.find('#phone').element.value).toBe('2222222222')
      expect(wrapper.find('#favorite').element.checked).toBe(false)
    })
  })
})
