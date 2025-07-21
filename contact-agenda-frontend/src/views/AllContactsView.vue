<template>
  <div class="all-contacts-view">
    <div class="page-header">
      <h1 class="page-title">Contact Agenda</h1>
      <p class="page-subtitle">Manage your contact list ({{ contactStore.contactCount }} contacts)</p>
      
      <!-- Search functionality -->
      <div class="search-container">
        <input 
          v-model="searchTerm" 
          type="text" 
          placeholder="Search contacts by name, email, or phone..."
          class="search-input"
        />
      </div>
    </div>
    
    <!-- Loading state -->
    <div v-if="contactStore.loading" class="loading">
      <p>Loading contacts...</p>
    </div>
    
    <!-- Error state -->
    <div v-else-if="contactStore.error" class="error-message">
      <p>{{ contactStore.error }}</p>
      <button @click="contactStore.fetchContacts(true)" class="btn btn-primary">Try Again</button>
      <button @click="contactStore.clearError" class="btn btn-secondary">Dismiss</button>
    </div>
    
    <!-- Empty state -->
    <div v-else-if="filteredContacts.length === 0 && contactStore.contactCount === 0" class="empty-state">
      <h3>No contacts found</h3>
      <p>Start by adding your first contact.</p>
      <router-link to="/add-contact" class="btn btn-primary">Add Contact</router-link>
    </div>
    
    <!-- No search results -->
    <div v-else-if="filteredContacts.length === 0 && searchTerm.trim()" class="empty-state">
      <h3>No contacts match your search</h3>
      <p>Try a different search term or <button @click="searchTerm = ''" class="link-button">clear search</button>.</p>
    </div>
    
    <!-- Contacts grid -->
    <div v-else class="contacts-grid">
      <ContactCard 
        v-for="contact in filteredContacts" 
        :key="contact.id" 
        :contact="contact"
        @updated="handleContactUpdated"
        @deleted="handleContactDeleted"
      />
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import ContactCard from '../components/ui/ContactCard.vue'
import { useContactStore } from '../stores/contactStore'

// Initialize the contact store
const contactStore = useContactStore()

// Local search functionality
const searchTerm = ref('')

onMounted(async () => {
  // Fetch contacts when component mounts
  // Store handles caching, so this won't make unnecessary API calls
  await contactStore.fetchContacts()
})

/**
 * Handle contact update from ContactCard component
 * The store automatically updates all reactive references
 * @param updatedContact - The updated contact data
 */
function handleContactUpdated(updatedContact) {
  // With Pinia store, this is handled automatically by the updateContact action
  // We don't need to manually update local state anymore
  console.log('Contact updated via store:', updatedContact.name)
}

/**
 * Handle contact deletion from ContactCard component
 * The store automatically removes from all reactive references
 * @param deletedId - The ID of the deleted contact
 */
function handleContactDeleted(deletedId) {
  // With Pinia store, this is handled automatically by the deleteContact action
  // We don't need to manually filter local state anymore
  console.log('Contact deleted via store:', deletedId)
}

/**
 * Get filtered contacts based on search term
 * Uses the store's computed search function
 */
const filteredContacts = computed(() => {
  return contactStore.searchContacts(searchTerm.value)
})
</script>

<style scoped>
.all-contacts-view {
  width: 100%;
  max-width: 100%;
  margin: 0 auto;
  padding: 2rem 1rem;
  box-sizing: border-box;
}

.page-header {
  text-align: center;
  margin-bottom: 2rem;
}

.page-title {
  color: #2d3748;
  font-size: 2rem;
  margin-bottom: 0.5rem;
}

.page-subtitle {
  color: #718096;
  font-size: 1.125rem;
  margin-bottom: 1.5rem;
}

.search-container {
  max-width: 400px;
  margin: 0 auto;
}

.search-input {
  width: 100%;
  padding: 0.75rem 1rem;
  border: 2px solid #e2e8f0;
  border-radius: 8px;
  font-size: 1rem;
  transition: border-color 0.3s ease;
}

.search-input:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.link-button {
  background: none;
  border: none;
  color: #667eea;
  text-decoration: underline;
  cursor: pointer;
  font-size: inherit;
}

.link-button:hover {
  color: #5a67d8;
}

.loading, .error-message, .empty-state {
  text-align: center;
  padding: 3rem 1rem;
}

.loading p {
  font-size: 1.125rem;
  color: #718096;
}

.error-message {
  background: #fed7d7;
  color: #e53e3e;
  border-radius: 8px;
  border: 1px solid #feb2b2;
}

.empty-state {
  background: white;
  border-radius: 8px;
  border: 1px solid #e2e8f0;
}

.empty-state h3 {
  color: #2d3748;
  margin-bottom: 0.5rem;
}

.empty-state p {
  color: #718096;
  margin-bottom: 1.5rem;
}

.contacts-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 1.5rem;
  width: 100%;
  max-width: 100%;
}

.btn {
  display: inline-block;
  padding: 0.75rem 1.5rem;
  background: #667eea;
  color: white;
  text-decoration: none;
  border: none;
  border-radius: 4px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.btn:hover {
  background: #5a67d8;
}

/* Responsive adjustments for smaller screens */
@media (max-width: 768px) {
  .contacts-grid {
    grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
    gap: 1rem;
  }
  
  .all-contacts-view {
    padding: 1rem 0.5rem;
  }
  
  .page-title {
    font-size: 1.5rem;
  }
}

@media (max-width: 480px) {
  .contacts-grid {
    grid-template-columns: 1fr;
    gap: 1rem;
  }
  
  .all-contacts-view {
    padding: 0.5rem;
  }
  
  .page-title {
    font-size: 1.25rem;
  }
}
</style>
