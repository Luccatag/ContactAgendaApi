<template>
  <div class="contact-agenda">
    <h2>Contact Agenda</h2>
    
    <!-- Error display -->
    <div v-if="error" class="error">
      {{ error }}
    </div>
    
    <!-- Loading indicator -->
    <div v-if="loading" class="loading">
      Loading...
    </div>
    
    <!-- Add contact form -->
    <form @submit.prevent="addContact" :disabled="loading">
      <input v-model="newContact.name" placeholder="Name" required :disabled="loading" />
      <input v-model="newContact.email" placeholder="Email" type="email" required :disabled="loading" />
      <input v-model="newContact.phone" placeholder="Phone" required :disabled="loading" />
      <button type="submit" :disabled="loading">Add Contact</button>
    </form>
    
    <!-- Contacts list -->
    <ul v-if="contacts.length > 0">
      <li v-for="contact in contacts" :key="contact.id" class="contact-item">
        <span>{{ contact.name }} - {{ contact.email }} - {{ contact.phone }}</span>
        <button @click="deleteContact(contact.id)" :disabled="loading" class="delete-btn">
          Delete
        </button>
      </li>
    </ul>
    
    <!-- Empty state -->
    <p v-else-if="!loading">No contacts found.</p>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { ContactService, type Contact, type ContactCreateDto } from '../services/contactService'

const contacts = ref<Contact[]>([])
const newContact = ref<ContactCreateDto>({ name: '', email: '', phone: '' })
const loading = ref(false)
const error = ref<string | null>(null)

async function fetchContacts() {
  try {
    loading.value = true
    error.value = null
    contacts.value = await ContactService.getAllContacts()
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Failed to fetch contacts'
  } finally {
    loading.value = false
  }
}

async function addContact() {
  try {
    loading.value = true
    error.value = null
    await ContactService.createContact(newContact.value)
    newContact.value = { name: '', email: '', phone: '' }
    await fetchContacts()
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Failed to add contact'
  } finally {
    loading.value = false
  }
}

async function deleteContact(id: number) {
  try {
    loading.value = true
    error.value = null
    await ContactService.deleteContact(id)
    await fetchContacts()
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Failed to delete contact'
  } finally {
    loading.value = false
  }
}

onMounted(fetchContacts)
</script>

<style scoped>
.contact-agenda { 
  max-width: 600px; 
  margin: 2rem auto; 
  padding: 1rem;
}

.error {
  background-color: #f8d7da;
  color: #721c24;
  padding: 0.75rem;
  border-radius: 0.25rem;
  margin-bottom: 1rem;
}

.loading {
  text-align: center;
  color: #6c757d;
  margin: 1rem 0;
}

form {
  display: flex;
  gap: 0.5rem;
  margin-bottom: 1rem;
  flex-wrap: wrap;
}

input {
  flex: 1;
  min-width: 150px;
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 0.25rem;
}

button {
  padding: 0.5rem 1rem;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 0.25rem;
  cursor: pointer;
}

button:hover {
  background-color: #0056b3;
}

button:disabled {
  background-color: #6c757d;
  cursor: not-allowed;
}

.contact-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.5rem;
  border-bottom: 1px solid #eee;
}

.delete-btn {
  background-color: #dc3545;
  padding: 0.25rem 0.5rem;
  font-size: 0.875rem;
}

.delete-btn:hover {
  background-color: #c82333;
}

ul {
  list-style: none;
  padding: 0;
  margin-top: 1rem;
}
</style>
