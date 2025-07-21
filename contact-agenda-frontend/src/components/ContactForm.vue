<template>
  <form @submit.prevent="onSave">
    <div class="form-field">
      <label>Name</label>
      <input v-model="localContact.name" type="text" required />
    </div>
    <div class="form-field">
      <label>Email</label>
      <input v-model="localContact.email" type="email" required @blur="validateEmail" />
      <div v-if="emailError" class="field-error">{{ emailError }}</div>
    </div>
    <div class="form-field">
      <label>Phone</label>
      <input v-model="localContact.phone" type="tel" required />
    </div>
    <button type="submit">Save</button>
    <button type="button" @click="$emit('cancel')" style="margin-left:0.5rem">Cancel</button>
  </form>
</template>

<script setup>
// ContactForm.vue: Used for both adding and editing a contact
// Props: contact (object, optional), emits: save, cancel
import { ref, watch } from 'vue'
const props = defineProps({ contact: Object })
const emit = defineEmits(['save', 'cancel'])
const localContact = ref({ ...props.contact })
const emailError = ref('')

watch(() => props.contact, (newVal) => {
  Object.assign(localContact.value, newVal)
})

function validateEmail() {
  // Improved regex: [something]@[something].[something] (at least one dot after @, at least one char before/after dot)
  const regex = /^[^@\s]+@[^@\s]+\.[^@\s\.]{2,}$/
  if (!regex.test(localContact.value.email)) {
    emailError.value = 'Invalid email format. Example: user@example.com'
    return false
  }
  emailError.value = ''
  return true
}

function onSave() {
  if (!validateEmail()) return
  emit('save', { ...localContact.value })
}
</script>
