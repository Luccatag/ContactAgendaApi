<template>
  <form @submit.prevent="handleSubmit" class="contact-form">
    <h2 v-if="title" class="form-title">{{ title }}</h2>
    
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
      <button type="submit" class="btn btn-primary" :disabled="loading || hasErrors">
        {{ loading ? 'Saving...' : (submitButtonText || 'Save Contact') }}
      </button>
      <button type="button" @click="resetForm" class="btn btn-secondary">
        Reset
      </button>
    </div>
    
    <div v-if="successMessage" class="success-message">
      {{ successMessage }}
    </div>
    
    <div v-if="errorMessage" class="error-message">
      {{ errorMessage }}
    </div>
  </form>
</template>

<script setup>
import { ref, reactive, computed } from 'vue'

const props = defineProps({
  title: {
    type: String,
    default: ''
  },
  submitButtonText: {
    type: String,
    default: 'Save Contact'
  }
})

const emit = defineEmits(['submitted', 'success'])

const loading = ref(false)
const successMessage = ref('')
const errorMessage = ref('')
const emailError = ref('')
const phoneError = ref('')

const formData = reactive({
  name: '',
  email: '',
  phone: ''
})

const hasErrors = computed(() => {
  return emailError.value || phoneError.value
})

const validateEmail = () => {
  const emailRegex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/
  if (formData.email && !emailRegex.test(formData.email)) {
    emailError.value = 'Please enter a valid email address (something@something.something)'
  } else {
    emailError.value = ''
  }
}

const validatePhone = () => {
  const phoneRegex = /^[\+]?[0-9\s\-\(\)]{10,}$/
  if (formData.phone && !phoneRegex.test(formData.phone)) {
    phoneError.value = 'Please enter a valid phone number (at least 10 digits)'
  } else {
    phoneError.value = ''
  }
}

const resetForm = () => {
  formData.name = ''
  formData.email = ''
  formData.phone = ''
  emailError.value = ''
  phoneError.value = ''
  successMessage.value = ''
  errorMessage.value = ''
}

const handleSubmit = async () => {
  // Validate before submitting
  validateEmail()
  validatePhone()
  
  if (hasErrors.value) {
    return
  }
  
  loading.value = true
  successMessage.value = ''
  errorMessage.value = ''
  
  try {
    const response = await fetch('/api/contacts', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(formData)
    })
    
    if (!response.ok) {
      const errorData = await response.text()
      throw new Error(errorData || 'Failed to add contact')
    }
    
    const newContact = await response.json()
    successMessage.value = `Contact "${newContact.name}" added successfully!`
    
    emit('submitted', newContact)
    emit('success', newContact)
    
    // Reset form after successful submission
    resetForm()
    
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'Failed to add contact'
    console.error('Error adding contact:', error)
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.contact-form {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  border: 1px solid #e2e8f0;
}

.form-title {
  margin: 0 0 1.5rem 0;
  color: #2d3748;
  font-size: 1.5rem;
  font-weight: 600;
}

.form-group {
  margin-bottom: 1.5rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: #2d3748;
}

.form-group input {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #e2e8f0;
  border-radius: 4px;
  font-size: 1rem;
  transition: border-color 0.3s ease, box-shadow 0.3s ease;
}

.form-group input:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.form-actions {
  display: flex;
  gap: 1rem;
  margin-top: 2rem;
}

.btn {
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 4px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.btn-primary {
  background: #667eea;
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background: #5a67d8;
}

.btn-primary:disabled {
  background: #a0aec0;
  cursor: not-allowed;
}

.btn-secondary {
  background: #e2e8f0;
  color: #4a5568;
}

.btn-secondary:hover {
  background: #cbd5e0;
}

.validation-error {
  color: #e53e3e;
  font-size: 0.875rem;
  margin-top: 0.25rem;
}

.success-message {
  color: #38a169;
  background: #c6f6d5;
  padding: 0.75rem;
  border-radius: 4px;
  margin-top: 1rem;
  border: 1px solid #9ae6b4;
}

.error-message {
  color: #e53e3e;
  background: #fed7d7;
  padding: 0.75rem;
  border-radius: 4px;
  margin-top: 1rem;
  border: 1px solid #feb2b2;
}
</style>
