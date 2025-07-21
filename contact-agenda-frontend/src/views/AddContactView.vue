<template>
  <div class="contact-agenda">
    <h2>Add New Contact</h2>
    <form @submit.prevent="addContact">
      <div class="form-field">
        <label>Name</label>
        <input v-model="name" type="text" required />
      </div>
      <div class="form-field">
        <label>Email</label>
        <input v-model="email" type="email" required />
      </div>
      <div class="form-field">
        <label>Phone</label>
        <input v-model="phone" type="tel" required />
      </div>
      <button type="submit">Add Contact</button>
    </form>
    <div v-if="success" class="success-message">Contact added!</div>
    <div v-if="error" class="error-message">{{ error }}</div>
    <button @click="$router.push('/')" style="margin-top:1rem">Back to Home</button>
  </div>
</template>

<script setup>
import { ref } from 'vue'
const name = ref('')
const email = ref('')
const phone = ref('')
const success = ref(false)
const error = ref('')

async function addContact() {
  success.value = false
  error.value = ''
  try {
    const res = await fetch('/api/contacts', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name: name.value, email: email.value, phone: phone.value })
    })
    if (!res.ok) throw new Error('Failed to add contact')
    name.value = ''
    email.value = ''
    phone.value = ''
    success.value = true
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
