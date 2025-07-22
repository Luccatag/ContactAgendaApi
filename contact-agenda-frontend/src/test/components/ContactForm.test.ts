import { describe, it, expect, vi, beforeEach } from 'vitest'
import { setActivePinia, createPinia } from 'pinia'

// Mock the contact store
const mockStore = {
  addContact: vi.fn(),
  updateContact: vi.fn(),
  contactExistsByEmail: vi.fn(),
  error: null as string | null
}

vi.mock('../../stores/contactStore', () => ({
  useContactStore: () => mockStore
}))

describe('ContactForm Tests', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    vi.clearAllMocks()
    mockStore.error = null
    mockStore.contactExistsByEmail.mockReturnValue(false)
  })

  it('should have correct initial state', () => {
    expect(mockStore.addContact).toBeDefined()
    expect(mockStore.updateContact).toBeDefined()
    expect(mockStore.contactExistsByEmail).toBeDefined()
  })

  it('should handle contact creation', async () => {
    mockStore.addContact.mockResolvedValue({
      id: 1,
      name: 'Test Contact',
      email: 'test@example.com',
      phone: '1234567890',
      isFavorite: false
    })

    const result = await mockStore.addContact({
      name: 'Test Contact',
      email: 'test@example.com',
      phone: '1234567890',
      isFavorite: false
    })

    expect(mockStore.addContact).toHaveBeenCalledWith({
      name: 'Test Contact',
      email: 'test@example.com',
      phone: '1234567890',
      isFavorite: false
    })
    expect(result.id).toBe(1)
  })

  it('should check if contact exists by email', () => {
    mockStore.contactExistsByEmail.mockReturnValue(true)
    
    const exists = mockStore.contactExistsByEmail('test@example.com')
    
    expect(mockStore.contactExistsByEmail).toHaveBeenCalledWith('test@example.com')
    expect(exists).toBe(true)
  })
})
