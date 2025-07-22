<template>
  <div class="add-contact-view">
    <h1>Add New Contact</h1>
    
    <form @submit.prevent="handleSubmit" class="contact-form">
      <div class="form-group">
        <label for="name">Name:</label>
        <input 
          id="name"
          v-model="formData.name" 
          type="text" 
          required 
          placeholder="Enter contact name"
        />
      </div>
      
      <div class="form-group">
        <label for="email">Email:</label>
        <input 
          id="email"
          v-model="formData.email" 
          type="email" 
          required 
          pattern="[^@\s]+@[^@\s]+\.[^@\s]+"
          title="Please enter a valid email address (something@something.something)"
          placeholder="Enter email address"
          @input="validateEmail"
        />
        <div v-if="emailError" class="validation-error">{{ emailError }}</div>
      </div>
      
      <div class="form-group">
        <label for="phone">Phone:</label>
        <input 
          id="phone"
          v-model="formData.phone" 
          type="tel" 
          required 
          pattern="[\+]?[0-9\s\-\(\)]{10,}"
          title="Please enter a valid phone number (at least 10 digits)"
          placeholder="Enter phone number"
          @input="validatePhone"
        />
        <div v-if="phoneError" class="validation-error">{{ phoneError }}</div>
      </div>
      
      <div class="form-actions">
        <button type="submit" class="btn btn-primary" :disabled="loading">
          {{ loading ? 'Adding...' : 'Add Contact' }}
        </button>
        <router-link to="/contacts" class="btn btn-secondary">Cancel</router-link>
      </div>
    </form>
    
    <div v-if="successMessage" class="success-message">
      {{ successMessage }}
    </div>
    
    <div v-if="errorMessage" class="error-message">
      {{ errorMessage }}
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useContactStore } from '../stores/contactStore'

// Initialize router and contact store
const router = useRouter()
const contactStore = useContactStore()

// Ensure contacts are loaded when component mounts
onMounted(async () => {
  await contactStore.fetchContacts()
})

interface FormData {
  name: string
  email: string
  phone: string
}

const loading = ref(false)
const successMessage = ref('')
const errorMessage = ref('')
const emailError = ref('')
const phoneError = ref('')

const formData = ref<FormData>({
  name: '',
  email: '',
  phone: ''
})

const validateEmail = () => {
  const emailRegex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/
  if (formData.value.email && !emailRegex.test(formData.value.email)) {
    emailError.value = 'Please enter a valid email address (something@something.something)'
  } else {
    emailError.value = ''
  }
}

const validatePhone = () => {
  const phoneRegex = /^[\+]?[0-9\s\-\(\)]{10,}$/
  if (formData.value.phone && !phoneRegex.test(formData.value.phone)) {
    phoneError.value = 'Please enter a valid phone number (at least 10 digits)'
  } else {
    phoneError.value = ''
  }
}

const handleSubmit = async () => {
  // Validate email and phone before submitting
  validateEmail()
  validatePhone()
  if (emailError.value || phoneError.value) {
    return
  }
  
  loading.value = true
  successMessage.value = ''
  errorMessage.value = ''
  
  try {
    // Check for duplicate email using store getter
    if (contactStore.contactExistsByEmail(formData.value.email)) {
      errorMessage.value = 'A contact with this email already exists'
      return
    }
    
    // Create contact using store action
    const newContact = await contactStore.addContact(formData.value)
    successMessage.value = `Contact "${newContact.name}" added successfully!`
    
    // Reset form
    formData.value = { name: '', email: '', phone: '' }
    
    // Redirect after 2 seconds to home page (favorites view)
    setTimeout(() => {
      router.push('/')
    }, 2000)
    
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'Failed to add contact'
    console.error('Error adding contact:', error)
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.add-contact-view {
  max-width: 500px;
  margin: 0 auto;
  padding: 20px;
}

.contact-form {
  background: white;
  padding: 30px;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.form-group {
  margin-bottom: 20px;
}

label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
  color: #333;
}

input {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 16px;
}

input:focus {
  outline: none;
  border-color: #007bff;
  box-shadow: 0 0 0 2px rgba(0, 123, 255, 0.25);
}

.form-actions {
  display: flex;
  gap: 10px;
  margin-top: 30px;
}

.btn {
  padding: 12px 24px;
  text-decoration: none;
  border-radius: 6px;
  font-weight: bold;
  border: none;
  cursor: pointer;
  display: inline-block;
  text-align: center;
}

.btn-primary {
  background: #007bff;
  color: white;
}

.btn-secondary {
  background: #6c757d;
  color: white;
}

.btn:hover {
  opacity: 0.8;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.success-message {
  color: green;
  padding: 10px;
  background: #e6ffe6;
  border-radius: 4px;
  margin-top: 20px;
}

.error-message {
  color: red;
  padding: 10px;
  background: #ffe6e6;
  border-radius: 4px;
  margin-top: 20px;
}

.validation-error {
  color: red;
  font-size: 14px;
  margin-top: 5px;
}
</style>
