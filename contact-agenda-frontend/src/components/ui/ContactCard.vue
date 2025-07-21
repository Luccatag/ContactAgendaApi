<template>
  <!-- Contact card container with conditional rendering for display/edit modes -->
  <div class="contact-card">
    
    <!-- Display mode: Shows contact information with action buttons -->
    <div v-if="!isEditing" class="contact-display">
      <div class="contact-info">
        <!-- Contact name with favorite heart button -->
        <div class="contact-header">
          <h3 class="contact-name">{{ contact.name }}</h3>
          <button 
            @click="toggleFavorite" 
            class="btn btn-favorite"
            :class="{ 'is-favorite': contact.isFavorite }"
            :title="contact.isFavorite ? 'Remove from favorites' : 'Add to favorites'"
          >
            {{ contact.isFavorite ? '❤️' : '🤍' }}
          </button>
        </div>
        
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

        <!-- Favorite checkbox -->
        <div class="form-group checkbox-group">
          <label class="checkbox-label">
            <input 
              v-model="editData.isFavorite" 
              type="checkbox"
              class="checkbox-input"
            />
            <span class="checkbox-text">⭐ Mark as favorite</span>
          </label>
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
import { useContactStore } from '../../stores/contactStore'

// Initialize the contact store
const contactStore = useContactStore()

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
  phone: '',
  isFavorite: false
})

/**
 * Switches the card to edit mode and populates the form with current contact data
 */
const startEdit = () => {
  isEditing.value = true
  editData.name = props.contact.name
  editData.email = props.contact.email
  editData.phone = props.contact.phone
  editData.isFavorite = props.contact.isFavorite
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
 * Uses Pinia store for centralized state management
 * Includes client-side validation for email and phone formats
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
    // Use store action instead of direct API call
    // Store handles optimistic updates and error recovery
    const updatedContact = await contactStore.updateContact(props.contact.id, {
      name: editData.name,
      email: editData.email,
      phone: editData.phone,
      isFavorite: editData.isFavorite
    })
    
    // Emit the updated contact data to parent component for any additional handling
    emit('updated', updatedContact)
    
    // Exit edit mode
    isEditing.value = false
  } catch (error) {
    // Error is already handled by the store, just display it
    editError.value = error.message || 'Failed to update contact'
  }
}

/**
 * Toggles the favorite status of the contact
 * Uses Pinia store for centralized state management with optimistic updates
 */
const toggleFavorite = async () => {
  try {
    // Use store action to toggle favorite status
    // Store handles optimistic updates and error recovery
    await contactStore.toggleFavorite(props.contact.id)
  } catch (error) {
    console.error('Error toggling favorite:', error)
    // Store already handles error display, but we could add user-facing feedback here
  }
}

/**
 * Deletes the contact after user confirmation
 * Uses Pinia store for centralized state management
 */
const deleteContact = async () => {
  // Show confirmation dialog
  if (!confirm(`Are you sure you want to delete ${props.contact.name}?`)) {
    return
  }
  
  try {
    // Use store action instead of direct API call
    // Store handles optimistic updates and error recovery
    await contactStore.deleteContact(props.contact.id)
    
    // Emit the deleted contact ID to parent component for any additional handling
    emit('deleted', props.contact.id)
  } catch (error) {
    console.error('Error deleting contact:', error)
    // Store already handles error display, but we could add user-facing feedback here
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

/* Header section with name and favorite button */
.contact-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 0.75rem;
}

.contact-name {
  margin: 0;
  color: #2d3748;
  font-size: 1.25rem;
  font-weight: 600;
  flex: 1;
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

/* Favorite button - heart emoji button */
.btn-favorite {
  background: transparent;
  border: none;
  font-size: 1.2rem;
  padding: 0.25rem;
  cursor: pointer;
  border-radius: 50%;
  width: 2rem;
  height: 2rem;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s ease;
  margin-left: 0.5rem;
}

.btn-favorite:hover {
  background: rgba(0, 0, 0, 0.05);
  transform: scale(1.1);
}

.btn-favorite.is-favorite {
  animation: pulse 0.3s ease;
}

@keyframes pulse {
  0% { transform: scale(1); }
  50% { transform: scale(1.2); }
  100% { transform: scale(1); }
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

/* Checkbox group styles */
.checkbox-group {
  margin-bottom: 1rem;
}

.checkbox-label {
  display: flex;
  align-items: center;
  cursor: pointer;
  font-weight: 500;
  color: #2d3748;
  font-size: 0.9rem;
}

.checkbox-input {
  width: auto !important;
  margin-right: 0.5rem;
  margin-bottom: 0;
  transform: scale(1.1);
  cursor: pointer;
}

.checkbox-text {
  user-select: none;
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
