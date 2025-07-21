/**
 * Test setup file for Vitest
 * 
 * This file is automatically run before each test suite and provides:
 * - Global test utilities and mocks
 * - DOM environment setup for Vue component testing
 * - Common test helpers and configurations
 */

import { vi } from 'vitest'

// Mock IntersectionObserver for testing environment
Object.defineProperty(globalThis, 'IntersectionObserver', {
  writable: true,
  configurable: true,
  value: class IntersectionObserver {
    constructor() {}
    disconnect() {}
    observe() {}
    unobserve() {}
  }
})

// Mock ResizeObserver for testing environment
Object.defineProperty(globalThis, 'ResizeObserver', {
  writable: true,
  configurable: true,
  value: class ResizeObserver {
    constructor() {}
    disconnect() {}
    observe() {}
    unobserve() {}
  }
})

// Setup fetch mock for API testing
Object.defineProperty(globalThis, 'fetch', {
  writable: true,
  configurable: true,
  value: vi.fn()
})

// Mock window.scrollTo
Object.defineProperty(window, 'scrollTo', {
  value: vi.fn(),
  writable: true
})

// Global test helpers
export const mockContact = {
  id: 1,
  name: 'Test Contact',
  email: 'test@example.com',
  phone: '1234567890',
  isFavorite: false
}

export const mockContacts = [
  mockContact,
  {
    id: 2,
    name: 'Jane Doe',
    email: 'jane@example.com',
    phone: '0987654321',
    isFavorite: true
  }
]
