<template>
  <div class="contact-agenda">
    <h2>Add New Contact</h2>
    <form @submit.prevent="addContact">
      <div class="form-field">
        <label>Name</label>
        <input v-model="name" type="text" required placeholder="e.g., Joarome Doe"/>
      </div>
      <div class="form-field">
        <label>Email</label>
        <input v-model="email" type="email" required placeholder="e.g., example@email.com" @blur="validateEmail" :class="{ error: emailError }" />
        <div v-if="emailError" class="field-error">{{ emailError }}</div>
      </div>
      <div class="form-field">
        <label>Phone</label>
        <input 
          v-model="phone" 
          type="tel" 
          required 
          :class="{ 'error': phoneError }"
          @blur="validatePhone"
          placeholder="e.g., 123-456-7890 or (123) 456-7890"
        />
        <div v-if="phoneError" class="field-error">{{ phoneError }}</div>
      </div>
      <button type="submit">Add Contact</button>
    </form>
    <div v-if="success" class="success-message">Contact added!</div>
    <div v-if="error" class="error-message">{{ error }}</div>
    <!-- Removed Back to Home button as requested -->
  </div>
</template>

<script setup>
import { ref } from 'vue'
const name = ref('')
const email = ref('')
const phone = ref('')
const success = ref(false)
const error = ref('')
const emailError = ref('')
const phoneError = ref('')

// Email validation function
function validateEmail() {
  const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/
  if (!email.value) {
    emailError.value = 'Email is required'
    return false
  }
  if (!emailPattern.test(email.value)) {
    emailError.value = 'Please enter a valid email address'
    return false
  }
  emailError.value = ''
  return true
}

// Phone validation function
function validatePhone() {
  const phonePattern = /^[\+]?[(]?[\d\s\-\(\)]{10,}$/
  const cleanPhone = phone.value.replace(/[\s\-\(\)]/g, '')
  
  if (!phone.value) {
    phoneError.value = 'Phone number is required'
    return false
  }
  
  if (cleanPhone.length < 10) {
    phoneError.value = 'Phone number must be at least 10 digits'
    return false
  }
  
  if (!phonePattern.test(phone.value)) {
    phoneError.value = 'Please enter a valid phone number'
    return false
  }
  
  phoneError.value = ''
  return true
}

async function addContact() {
  success.value = false
  error.value = ''
  
  // Validate phone before submitting
  if (!validatePhone()) {
    return
  }

  // Validate email before submitting
  if (!validateEmail()) {
    return
  }
  
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
    phoneError.value = ''
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

.form-field {
  margin-bottom: 1rem;
}

.form-field label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
}

.form-field input {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
}

.form-field input.error {
  border-color: #ef4444;
  background-color: #fef2f2;
}

.field-error {
  color: #ef4444;
  font-size: 0.875rem;
  margin-top: 0.25rem;
}

.success-message {
  color: #22c55e;
  background-color: #f0fdf4;
  padding: 0.75rem;
  border-radius: 4px;
  margin-top: 1rem;
}

.error-message {
  color: #ef4444;
  background-color: #fef2f2;
  padding: 0.75rem;
  border-radius: 4px;
  margin-top: 1rem;
}

button {
  background-color: #3b82f6;
  color: white;
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1rem;
}

button:hover {
  background-color: #2563eb;
}
</style>
