<template>
  <div class="contact-agenda">
    <h2>All Contacts (from JSON)</h2>
    <button @click="$router.push('/')">Back to Home</button>
    <div v-if="loading" class="loading">Loading contacts...</div>
    <div v-else-if="error" class="error-message">{{ error }}</div>
    <ul v-else>
      <li v-for="contact in contacts" :key="contact.id" class="contact-item">
        <div v-if="editId !== contact.id">
          <strong>{{ contact.name }}</strong>
          <div class="contact-details">
            Email: {{ contact.email }}<br />
            Phone: {{ contact.phone }}
          </div>
          <button @click="startEdit(contact)" style="margin-top:0.5rem">Edit</button>
        </div>
        <div v-else>
          <form @submit.prevent="saveEdit(contact.id)">
            <div class="form-field">
              <label>Name</label>
              <input v-model="editName" type="text" required />
            </div>
            <div class="form-field">
              <label>Email</label>
              <input 
                v-model="editEmail" 
                type="email" 
                pattern="[^@\s]+@[^@\s]+\.[^@\s]+"
                title="Please enter a valid email address (something@something.something)"
                required 
              />
            </div>
            <div class="form-field">
              <label>Phone</label>
              <input 
                v-model="editPhone" 
                type="tel" 
                pattern="[\+]?[0-9\s\-\(\)]{10,}"
                title="Please enter a valid phone number (at least 10 digits)"
                required 
              />
            </div>
            <button type="submit">Save</button>
            <button type="button" @click="cancelEdit" style="margin-left:0.5rem">Cancel</button>
          </form>
          <div v-if="editError" class="error-message">{{ editError }}</div>
        </div>
      </li>
    </ul>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const contacts = ref([])
const loading = ref(true)
const error = ref('')

// Edit state
const editId = ref(null)
const editName = ref('')
const editEmail = ref('')
const editPhone = ref('')
const editError = ref('')

onMounted(fetchContacts)

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

function startEdit(contact) {
  editId.value = contact.id
  editName.value = contact.name
  editEmail.value = contact.email
  editPhone.value = contact.phone
  editError.value = ''
}

function cancelEdit() {
  editId.value = null
  editName.value = ''
  editEmail.value = ''
  editPhone.value = ''
  editError.value = ''
}

async function saveEdit(id) {
  editError.value = ''
  
  // Validate email format
  const emailRegex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/
  if (!emailRegex.test(editEmail.value)) {
    editError.value = 'Please enter a valid email address (something@something.something)'
    return
  }
  
  // Validate phone format
  const phoneRegex = /^[\+]?[0-9\s\-\(\)]{10,}$/
  if (!phoneRegex.test(editPhone.value)) {
    editError.value = 'Please enter a valid phone number (at least 10 digits)'
    return
  }
  
  try {
    const res = await fetch(`/api/contacts/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ id, name: editName.value, email: editEmail.value, phone: editPhone.value })
    })
    if (!res.ok) throw new Error('Failed to update contact')
    // Update local list
    const idx = contacts.value.findIndex(c => c.id === id)
    if (idx !== -1) {
      contacts.value[idx] = { id, name: editName.value, email: editEmail.value, phone: editPhone.value }
    }
    cancelEdit()
  } catch (e) {
    editError.value = e.message
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
