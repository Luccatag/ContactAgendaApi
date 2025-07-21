import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { ContactService, type Contact, type ContactCreateDto } from '../services/contactService'

/**
 * Contact Store - Centralized state management for contact data
 * 
 * This store manages all contact-related state including:
 * - Contact list data
 * - Loading states
 * - Error handling
 * - CRUD operations
 * 
 * Benefits:
 * - Single source of truth for contact data
 * - Automatic reactivity across all components
 * - Centralized error handling
 * - Reduced API calls through caching
 * - Better user experience with optimistic updates
 */
export const useContactStore = defineStore('contacts', () => {
  // ==========================================
  // STATE - Reactive data that changes over time
  // ==========================================
  
  /**
   * Array of all contacts loaded from the server
   * This is the single source of truth for contact data
   */
  const contacts = ref<Contact[]>([])
  
  /**
   * Loading state for async operations
   * True when any API operation is in progress
   */
  const loading = ref(false)
  
  /**
   * Error message for displaying to users
   * Null when no error, string when error occurs
   */
  const error = ref<string | null>(null)
  
  /**
   * Currently selected contact for detailed operations
   * Used for edit modals, detailed views, etc.
   */
  const selectedContact = ref<Contact | null>(null)
  
  /**
   * Tracks if data has been loaded at least once
   * Prevents unnecessary API calls on subsequent component mounts
   */
  const isInitialized = ref(false)

  // ==========================================
  // GETTERS - Computed properties based on state
  // ==========================================
  
  /**
   * Total number of contacts
   * Reactive computed property that updates automatically
   */
  const contactCount = computed(() => contacts.value.length)
  
  /**
   * Contacts sorted with favorites first, then alphabetically by name
   * Favorites appear at the top of the list, sorted alphabetically among themselves
   * Non-favorites appear below, also sorted alphabetically
   */
  const sortedContacts = computed(() => {
    const sorted = [...contacts.value].sort((a, b) => a.name.localeCompare(b.name))
    
    // Separate favorites from non-favorites
    const favorites = sorted.filter(contact => contact.isFavorite)
    const nonFavorites = sorted.filter(contact => !contact.isFavorite)
    
    // Return favorites first, then non-favorites
    return [...favorites, ...nonFavorites]
  })
  
  /**
   * Contacts filtered by search term
   * Returns contacts that match name, email, or phone
   */
  const searchContacts = computed(() => {
    return (searchTerm: string) => {
      if (!searchTerm.trim()) return sortedContacts.value
      
      const term = searchTerm.toLowerCase()
      return sortedContacts.value.filter(contact =>
        contact.name.toLowerCase().includes(term) ||
        contact.email.toLowerCase().includes(term) ||
        contact.phone.toLowerCase().includes(term)
      )
    }
  })
  
  /**
   * Check if a contact with given email already exists
   * Useful for validation before creating new contacts
   */
  const contactExistsByEmail = computed(() => {
    return (email: string) => {
      return contacts.value.some(contact => 
        contact.email.toLowerCase() === email.toLowerCase()
      )
    }
  })

  // ==========================================
  // ACTIONS - Methods that modify state
  // ==========================================
  
  /**
   * Fetch all contacts from the API
   * Only fetches if not already initialized to avoid unnecessary calls
   * 
   * @param forceRefresh - If true, fetches data even if already initialized
   */
  const fetchContacts = async (forceRefresh = false) => {
    // Skip if already loaded and not forcing refresh
    if (isInitialized.value && !forceRefresh) {
      return
    }
    
    loading.value = true
    error.value = null
    
    try {
      const fetchedContacts = await ContactService.getAllContacts()
      contacts.value = fetchedContacts
      isInitialized.value = true
      
      console.log(`✅ Loaded ${fetchedContacts.length} contacts from API`)
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to fetch contacts'
      error.value = errorMessage
      console.error('❌ Error fetching contacts:', err)
    } finally {
      loading.value = false
    }
  }
  
  /**
   * Add a new contact
   * Uses optimistic updates for better UX
   * 
   * @param contactData - The contact data to create
   * @returns Promise<Contact> - The created contact with server-assigned ID
   */
  const addContact = async (contactData: ContactCreateDto): Promise<Contact> => {
    loading.value = true
    error.value = null
    
    try {
      // Make API call to create contact
      const newContact = await ContactService.createContact(contactData)
      
      // Add to local state (optimistic update)
      contacts.value.push(newContact)
      
      console.log(`✅ Added contact: ${newContact.name}`)
      return newContact
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to add contact'
      error.value = errorMessage
      console.error('❌ Error adding contact:', err)
      throw err
    } finally {
      loading.value = false
    }
  }
  
  /**
   * Update an existing contact
   * Uses optimistic updates for immediate UI feedback
   * 
   * @param id - Contact ID to update
   * @param contactData - Updated contact data
   * @returns Promise<Contact> - The updated contact
   */
  const updateContact = async (id: number, contactData: ContactCreateDto): Promise<Contact> => {
    loading.value = true
    error.value = null
    
    // Store original contact for rollback if needed
    const originalContact = contacts.value.find(c => c.id === id)
    const contactIndex = contacts.value.findIndex(c => c.id === id)
    
    if (contactIndex === -1) {
      throw new Error('Contact not found')
    }
    
    try {
      // Optimistic update - update UI immediately
      const optimisticContact = { id, ...contactData, isFavorite: contactData.isFavorite ?? false }
      contacts.value[contactIndex] = optimisticContact
      
      // Make API call
      const updatedContact = await ContactService.updateContact(id, contactData)
      
      // Update with server response
      contacts.value[contactIndex] = updatedContact
      
      // Update selected contact if it's the one being edited
      if (selectedContact.value?.id === id) {
        selectedContact.value = updatedContact
      }
      
      console.log(`✅ Updated contact: ${updatedContact.name}`)
      return updatedContact
    } catch (err) {
      // Rollback optimistic update on error
      if (originalContact) {
        contacts.value[contactIndex] = originalContact
      }
      
      const errorMessage = err instanceof Error ? err.message : 'Failed to update contact'
      error.value = errorMessage
      console.error('❌ Error updating contact:', err)
      throw err
    } finally {
      loading.value = false
    }
  }
  
  /**
   * Delete a contact
   * Removes from local state immediately for better UX
   * 
   * @param id - Contact ID to delete
   */
  const deleteContact = async (id: number): Promise<void> => {
    loading.value = true
    error.value = null
    
    // Store original contact for rollback if needed
    const originalContacts = [...contacts.value]
    const contactToDelete = contacts.value.find(c => c.id === id)
    
    try {
      // Optimistic update - remove from UI immediately
      contacts.value = contacts.value.filter(c => c.id !== id)
      
      // Clear selected contact if it's the one being deleted
      if (selectedContact.value?.id === id) {
        selectedContact.value = null
      }
      
      // Make API call
      await ContactService.deleteContact(id)
      
      console.log(`✅ Deleted contact: ${contactToDelete?.name || 'Unknown'}`)
    } catch (err) {
      // Rollback optimistic update on error
      contacts.value = originalContacts
      
      const errorMessage = err instanceof Error ? err.message : 'Failed to delete contact'
      error.value = errorMessage
      console.error('❌ Error deleting contact:', err)
      throw err
    } finally {
      loading.value = false
    }
  }
  
  /**
   * Toggle the favorite status of a contact
   * Updates the contact locally first (optimistic update) then syncs with server
   * 
   * @param id - ID of the contact to toggle favorite status
   */
  const toggleFavorite = async (id: number) => {
    const contactIndex = contacts.value.findIndex(c => c.id === id)
    
    if (contactIndex === -1) {
      throw new Error('Contact not found')
    }
    
    const originalContact = { ...contacts.value[contactIndex] }
    
    try {
      loading.value = true
      error.value = null
      
      // Optimistic update - toggle favorite status immediately
      contacts.value[contactIndex].isFavorite = !contacts.value[contactIndex].isFavorite
      
      console.log(`⭐ Toggling favorite for: ${originalContact.name} (${contacts.value[contactIndex].isFavorite ? 'favorited' : 'unfavorited'})`)
      
      // Make API call
      const updatedContact = await ContactService.toggleFavorite(id)
      
      // Update with server response
      contacts.value[contactIndex] = updatedContact
      
      // Update selected contact if it's the one being toggled
      if (selectedContact.value?.id === id) {
        selectedContact.value = updatedContact
      }
      
      console.log(`✅ Favorite toggled for: ${updatedContact.name}`)
    } catch (err) {
      // Rollback optimistic update on error
      contacts.value[contactIndex] = originalContact
      
      const errorMessage = err instanceof Error ? err.message : 'Failed to toggle favorite'
      error.value = errorMessage
      console.error('❌ Error toggling favorite:', err)
      throw err
    } finally {
      loading.value = false
    }
  }
  
  /**
   * Select a contact for detailed operations
   * Used by components that need to work with a specific contact
   * 
   * @param contact - Contact to select, or null to clear selection
   */
  const setSelectedContact = (contact: Contact | null) => {
    selectedContact.value = contact
    console.log(`📌 Selected contact: ${contact?.name || 'None'}`)
  }
  
  /**
   * Find a contact by ID
   * Useful for getting contact details from just an ID
   * 
   * @param id - Contact ID to find
   * @returns Contact if found, undefined otherwise
   */
  const getContactById = (id: number): Contact | undefined => {
    return contacts.value.find(c => c.id === id)
  }
  
  /**
   * Clear all error messages
   * Useful for dismissing error notifications
   */
  const clearError = () => {
    error.value = null
  }
  
  /**
   * Reset the entire store to initial state
   * Useful for logout or data refresh scenarios
   */
  const resetStore = () => {
    contacts.value = []
    loading.value = false
    error.value = null
    selectedContact.value = null
    isInitialized.value = false
    console.log('🔄 Contact store reset')
  }

  // ==========================================
  // RETURN - Expose state and actions to components
  // ==========================================
  
  return {
    // State
    contacts,
    loading,
    error,
    selectedContact,
    isInitialized,
    
    // Getters
    contactCount,
    sortedContacts,
    searchContacts,
    contactExistsByEmail,
    
    // Actions
    fetchContacts,
    addContact,
    updateContact,
    deleteContact,
    toggleFavorite,
    setSelectedContact,
    getContactById,
    clearError,
    resetStore
  }
})

/**
 * Type helper for the contact store
 * Useful for TypeScript consumers of the store
 */
export type ContactStore = ReturnType<typeof useContactStore>
