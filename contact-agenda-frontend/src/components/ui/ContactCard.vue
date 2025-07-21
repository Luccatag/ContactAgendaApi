<template>
  <!-- Contact card container with conditional rendering for display/edit modes -->
  <div class="contact-card">
    
    <!-- Display mode: Shows contact information with action buttons -->
    <div v-if="!isEditing" class="contact-display">
      <div class="contact-info">
        <!-- Contact name as the main heading -->
        <h3 class="contact-name">{{ contact.name }}</h3>
        
        <!-- Contact details section -->
        <div class="contact-details">
          <p class="contact-email">
            <span class="label">Email:</span> {{ contact.email }}
          </p>
          <p class="contact-phone">
            <span class="label">Phone:</span> {{ contact.phone }}
          </p>
        </div>
      </div>
      
      <!-- Action buttons aligned to the right -->
      <div class="contact-actions">
        <button @click="startEdit" class="btn btn-edit">Edit</button>
        <button @click="deleteContact" class="btn btn-delete">Delete</button>
      </div>
    </div>

    <!-- Edit mode: Shows form for editing contact information -->
    <div v-else class="contact-edit">
      <form @submit.prevent="saveEdit">
        <!-- Name input field -->
        <div class="form-group">
          <label>Name:</label>
          <input v-model="editData.name" type="text" required />
        </div>
        
        <!-- Email input field with validation pattern -->
        <div class="form-group">
          <label>Email:</label>
          <input 
            v-model="editData.email" 
            type="email" 
            pattern="[^@\s]+@[^@\s]+\.[^@\s]+"
            title="Please enter a valid email address (something@something.something)"
            required 
          />
        </div>
        
        <!-- Phone input field with validation pattern -->
        <div class="form-group">
          <label>Phone:</label>
          <input 
            v-model="editData.phone" 
            type="tel" 
            pattern="[\+]?[0-9\s\-\(\)]{10,}"
            title="Please enter a valid phone number (at least 10 digits)"
            required 
          />
        </div>
        
        <!-- Form action buttons -->
        <div class="form-actions">
          <button type="submit" class="btn btn-save">Save</button>
          <button type="button" @click="cancelEdit" class="btn btn-cancel">Cancel</button>
        </div>
      </form>
      
      <!-- Error message display -->
      <div v-if="editError" class="error-message">{{ editError }}</div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'

/**
 * Props definition
 * @param {Object} contact - The contact object containing id, name, email, and phone
 */
const props = defineProps({
  contact: {
    type: Object,
    required: true
  }
})

/**
 * Events that this component can emit
 * @event updated - Emitted when a contact is successfully updated
 * @event deleted - Emitted when a contact is successfully deleted
 */
const emit = defineEmits(['updated', 'deleted'])

// Component state
const isEditing = ref(false) // Controls whether the card is in edit mode
const editError = ref('') // Stores any error messages during editing

// Reactive object to hold form data during editing
const editData = reactive({
  name: '',
  email: '',
  phone: ''
})

/**
 * Switches the card to edit mode and populates the form with current contact data
 */
const startEdit = () => {
  isEditing.value = true
  editData.name = props.contact.name
  editData.email = props.contact.email
  editData.phone = props.contact.phone
  editError.value = ''
}

/**
 * Cancels the edit operation and returns to display mode
 */
const cancelEdit = () => {
  isEditing.value = false
  editError.value = ''
}

/**
 * Validates and saves the edited contact data
 * Includes client-side validation for email and phone formats
 * Makes API call to update the contact on the server
 */
const saveEdit = async () => {
  editError.value = ''
  
  // Validate email format using regex
  // Pattern requires: characters@characters.characters
  const emailRegex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/
  if (!emailRegex.test(editData.email)) {
    editError.value = 'Please enter a valid email address (something@something.something)'
    return
  }
  
  // Validate phone format using regex
  // Pattern allows: optional +, digits, spaces, hyphens, parentheses (minimum 10 characters)
  const phoneRegex = /^[\+]?[0-9\s\-\(\)]{10,}$/
  if (!phoneRegex.test(editData.phone)) {
    editError.value = 'Please enter a valid phone number (at least 10 digits)'
    return
  }
  
  try {
    // Make API call to update the contact
    const response = await fetch(`/api/contacts/${props.contact.id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        id: props.contact.id,
        name: editData.name,
        email: editData.email,
        phone: editData.phone
      })
    })
    
    if (!response.ok) {
      throw new Error('Failed to update contact')
    }
    
    // Emit the updated contact data to parent component
    emit('updated', {
      id: props.contact.id,
      name: editData.name,
      email: editData.email,
      phone: editData.phone
    })
    
    // Exit edit mode
    isEditing.value = false
  } catch (error) {
    editError.value = error.message
  }
}

/**
 * Deletes the contact after user confirmation
 * Makes API call to delete the contact from the server
 */
const deleteContact = async () => {
  // Show confirmation dialog
  if (!confirm(`Are you sure you want to delete ${props.contact.name}?`)) {
    return
  }
  
  try {
    // Make API call to delete the contact
    const response = await fetch(`/api/contacts/${props.contact.id}`, {
      method: 'DELETE'
    })
    
    if (!response.ok) {
      throw new Error('Failed to delete contact')
    }
    
    // Emit the deleted contact ID to parent component
    emit('deleted', props.contact.id)
  } catch (error) {
    console.error('Error deleting contact:', error)
    // Could add user-facing error handling here
  }
}
</script>

<style scoped>
/* Main card container with hover effects */
.contact-card {
  background: white;
  border-radius: 8px;
  padding: 1.5rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  border: 1px solid #e2e8f0;
  transition: box-shadow 0.3s ease;
}

.contact-card:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

/* Contact display styles */
.contact-display {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 1rem;
}

.contact-info {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.contact-name {
  margin: 0 0 0.75rem 0;
  color: #2d3748;
  font-size: 1.25rem;
  font-weight: 600;
}

.contact-details {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.contact-details p {
  margin: 0;
  color: #4a5568;
  line-height: 1.4;
}

/* Label styling for field names */
.label {
  font-weight: 500;
  color: #2d3748;
}

/* Action buttons container - aligned to the right */
.contact-actions {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  flex-shrink: 0;
  align-self: flex-start;
}

/* Form styling for edit mode */
.form-group {
  margin-bottom: 1rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: #2d3748;
}

.form-group input {
  width: 100%;
  padding: 0.5rem;
  border: 1px solid #e2e8f0;
  border-radius: 4px;
  font-size: 1rem;
}

.form-group input:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

/* Form actions container */
.form-actions {
  display: flex;
  gap: 0.5rem;
  margin-top: 1rem;
}

/* Base button styles */
.btn {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.875rem;
  font-weight: 500;
  transition: background-color 0.3s ease;
}

/* Edit button - primary blue color */
.btn-edit {
  background: #667eea;
  color: white;
}

.btn-edit:hover {
  background: #5a67d8;
}

/* Delete button - danger red color */
.btn-delete {
  background: #e53e3e;
  color: white;
}

.btn-delete:hover {
  background: #c53030;
}

/* Save button - success green color */
.btn-save {
  background: #38a169;
  color: white;
}

.btn-save:hover {
  background: #2f855a;
}

/* Cancel button - neutral gray color */
.btn-cancel {
  background: #a0aec0;
  color: white;
}

.btn-cancel:hover {
  background: #718096;
}

/* Error message styling */
.error-message {
  color: #e53e3e;
  font-size: 0.875rem;
  margin-top: 0.5rem;
  padding: 0.5rem;
  background: #fed7d7;
  border-radius: 4px;
}

/* Responsive design for mobile devices */
@media (max-width: 480px) {
  .contact-display {
    flex-direction: column;
    align-items: stretch;
  }
  
  .contact-actions {
    flex-direction: row;
    justify-content: flex-end;
    margin-top: 1rem;
  }
  
  .btn {
    min-width: 70px;
  }
}
</style>
