<template>
  <div class="all-contacts-view">
    <div class="page-header">
      <h1 class="page-title">Contact Agenda</h1>
      <p class="page-subtitle">Manage your contact list</p>
    </div>
    
    <div v-if="loading" class="loading">
      <p>Loading contacts...</p>
    </div>
    
    <div v-else-if="error" class="error-message">
      <p>{{ error }}</p>
      <button @click="fetchContacts" class="btn btn-primary">Try Again</button>
    </div>
    
    <div v-else-if="contacts.length === 0" class="empty-state">
      <h3>No contacts found</h3>
      <p>Start by adding your first contact.</p>
      <router-link to="/add-contact" class="btn btn-primary">Add Contact</router-link>
    </div>
    
    <div v-else class="contacts-grid">
      <ContactCard 
        v-for="contact in contacts" 
        :key="contact.id" 
        :contact="contact"
        @updated="handleContactUpdated"
        @deleted="handleContactDeleted"
      />
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import ContactCard from '../components/ui/ContactCard.vue'
import { ContactService } from '../services/contactService'

const contacts = ref([])
const loading = ref(true)
const error = ref('')

onMounted(fetchContacts)

/**
 * Fetch all contacts using the ContactService
 */
async function fetchContacts() {
  loading.value = true
  error.value = ''
  
  try {
    contacts.value = await ContactService.getAllContacts()
  } catch (e) {
    error.value = e.message || 'Failed to fetch contacts'
    console.error('Error fetching contacts:', e)
  } finally {
    loading.value = false
  }
}

function handleContactUpdated(updatedContact) {
  const index = contacts.value.findIndex(c => c.id === updatedContact.id)
  if (index !== -1) {
    contacts.value[index] = updatedContact
  }
}

function handleContactDeleted(deletedId) {
  contacts.value = contacts.value.filter(c => c.id !== deletedId)
}
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
