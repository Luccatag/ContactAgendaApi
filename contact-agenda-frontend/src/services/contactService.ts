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

// Use relative API base URL so Vite proxy works
const API_BASE_URL = '/api'

export class ContactService {
  // Get all contacts
  static async getAllContacts(): Promise<Contact[]> {
    const response = await fetch(`${API_BASE_URL}/contactagenda`)
    if (!response.ok) {
      throw new Error(`Failed to fetch contacts: ${response.statusText}`)
    }
    return response.json()
  }

  // Get contact by ID
  static async getContactById(id: number): Promise<Contact> {
    const response = await fetch(`${API_BASE_URL}/contactagenda/${id}`)
    if (!response.ok) {
      throw new Error(`Failed to fetch contact: ${response.statusText}`)
    }
    return response.json()
  }

  // Create new contact
  static async createContact(contact: ContactCreateDto): Promise<Contact> {
    const response = await fetch(`${API_BASE_URL}/contactagenda`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(contact)
    })
    if (!response.ok) {
      throw new Error(`Failed to create contact: ${response.statusText}`)
    }
    return response.json()
  }

  // Update contact
  static async updateContact(id: number, contact: ContactCreateDto): Promise<Contact> {
    const response = await fetch(`${API_BASE_URL}/contactagenda/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ id, ...contact })
    })
    if (!response.ok) {
      throw new Error(`Failed to update contact: ${response.statusText}`)
    }
    return response.json()
  }

  // Delete contact
  static async deleteContact(id: number): Promise<void> {
    const response = await fetch(`${API_BASE_URL}/contactagenda/${id}`, {
      method: 'DELETE'
    })
    if (!response.ok) {
      throw new Error(`Failed to delete contact: ${response.statusText}`)
    }
  }
}
