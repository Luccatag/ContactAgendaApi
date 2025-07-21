import axios from 'axios'
import type { AxiosResponse } from 'axios'

// Type definitions that match your backend DTOs
export interface Contact {
  id: number
  name: string
  email: string
  phone: string
}

export interface ContactCreateDto {
  name: string
  email: string
  phone: string
}

// Configure axios instance with base URL and default settings
const apiClient = axios.create({
  baseURL: '/api',
  timeout: 10000, // 10 second timeout
  headers: {
    'Content-Type': 'application/json',
  },
})

// Add response interceptor for better error handling
apiClient.interceptors.response.use(
  (response: AxiosResponse) => response,
  (error) => {
    // Enhanced error handling
    if (error.response) {
      // Server responded with error status
      const message = error.response.data?.message || error.response.statusText || 'Server error'
      throw new Error(`${error.response.status}: ${message}`)
    } else if (error.request) {
      // Request was made but no response received
      throw new Error('Network error: No response from server')
    } else {
      // Something else happened
      throw new Error(`Request failed: ${error.message}`)
    }
  }
)

export class ContactService {
  /**
   * Get all contacts from the API
   * @returns Promise<Contact[]> Array of all contacts
   */
  static async getAllContacts(): Promise<Contact[]> {
    try {
      const response = await apiClient.get<Contact[]>('/contacts')
      return response.data
    } catch (error) {
      console.error('Error fetching all contacts:', error)
      throw error
    }
  }

  /**
   * Get a specific contact by ID
   * @param id - Contact ID to fetch
   * @returns Promise<Contact> The requested contact
   */
  static async getContactById(id: number): Promise<Contact> {
    try {
      const response = await apiClient.get<Contact>(`/contacts/${id}`)
      return response.data
    } catch (error) {
      console.error(`Error fetching contact ${id}:`, error)
      throw error
    }
  }

  /**
   * Create a new contact
   * @param contact - Contact data to create
   * @returns Promise<Contact> The created contact with ID
   */
  static async createContact(contact: ContactCreateDto): Promise<Contact> {
    try {
      const response = await apiClient.post<Contact>('/contacts', contact)
      return response.data
    } catch (error) {
      console.error('Error creating contact:', error)
      throw error
    }
  }

  /**
   * Update an existing contact
   * @param id - Contact ID to update
   * @param contact - Updated contact data
   * @returns Promise<Contact> The updated contact
   */
  static async updateContact(id: number, contact: ContactCreateDto): Promise<Contact> {
    try {
      const response = await apiClient.put<Contact>(`/contacts/${id}`, { id, ...contact })
      return response.data
    } catch (error) {
      console.error(`Error updating contact ${id}:`, error)
      throw error
    }
  }

  /**
   * Delete a contact by ID
   * @param id - Contact ID to delete
   * @returns Promise<void>
   */
  static async deleteContact(id: number): Promise<void> {
    try {
      await apiClient.delete(`/contacts/${id}`)
    } catch (error) {
      console.error(`Error deleting contact ${id}:`, error)
      throw error
    }
  }
}
