<template>
  <div class="contact-agenda">
    <h2>Contact Agenda</h2>
    <button @click="goToAddContact" style="margin-bottom:1rem">Add New Contact</button>
    <div v-if="loading" class="loading">Loading contacts...</div>
    <div v-else-if="error" class="error-message">{{ error }}</div>
    <ContactList
      v-if="!loading && contacts.length > 0"
      :contacts="contacts"
      @edit="handleEdit"
      @delete="handleDelete"
    />
    <p v-else-if="!loading">No contacts found.</p>
    <!-- Add Contact Modal/Section will go here -->
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import ContactList from '../components/ContactList.vue'

// State
const contacts = ref([])
const loading = ref(true)
const error = ref('')
// No need for showAdd, navigation will be used
// Navigate to add contact form
function goToAddContact() {
  // Assumes you have a route named 'AddContact' or '/add'
  // Adjust as needed for your router setup
  // Example: this.$router.push('/add-contact') for options API
  // For script setup:
  window.location.href = '/add-contact' // fallback if router not injected
}

onMounted(fetchContacts)

// Fetch contacts from backend
async function fetchContacts() {
  loading.value = true
  try {
    const res = await fetch('/api/contacts')
    if (!res.ok) throw new Error('Failed to fetch contacts')
    contacts.value = await res.json()
  } catch (e) {
    error.value = e.message
  } finally {
    loading.value = false
  }
}

// Handle edit event from ContactList
async function handleEdit(updated) {
  try {
    const res = await fetch(`/api/contacts/${updated.id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(updated)
    })
    if (!res.ok) throw new Error('Failed to update contact')
    // Update local list
    const idx = contacts.value.findIndex(c => c.id === updated.id)
    if (idx !== -1) {
      contacts.value[idx] = updated
    }
  } catch (e) {
    error.value = e.message
  }
}

// Handle delete event from ContactList
async function handleDelete(id) {
  try {
    const res = await fetch(`/api/contacts/${id}`, {
      method: 'DELETE'
    })
    if (!res.ok) throw new Error('Failed to delete contact')
    contacts.value = contacts.value.filter(c => c.id !== id)
  } catch (e) {
    error.value = e.message
  }
}
</script>

<style scoped>
.contact-agenda {
  max-width: 600px;
  margin: 2rem auto;
  padding: 2rem;
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}
</style>
