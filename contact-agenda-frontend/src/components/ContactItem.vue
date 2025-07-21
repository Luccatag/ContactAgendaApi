<template>
  <div class="contact-item">
    <div v-if="!editing">
      <strong>{{ contact.name }}</strong>
      <div class="contact-details">
        Email: {{ contact.email }}<br />
        Phone: {{ contact.phone }}
      </div>
      <button @click="editing = true">Edit</button>
      <button @click="$emit('delete', contact.id)" style="margin-left:0.5rem">Delete</button>
    </div>
    <div v-else>
      <ContactForm :contact="contact" @save="saveEdit" @cancel="editing = false" />
    </div>
  </div>
</template>
<style scoped>
/* Card style for grid layout */
.contact-item {
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.07);
  padding: 1rem;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  max-width: 350px;
  width: 100%;
  box-sizing: border-box;
}
</style>

<script setup>
// ContactItem.vue: Renders a single contact, allows edit/delete
// Props: contact (object), emits: delete
import { ref } from 'vue'
import ContactForm from './ContactForm.vue'
const props = defineProps({ contact: Object })
const editing = ref(false)
function saveEdit(updated) {
  editing.value = false
  // Emit edit event to parent
  emit('edit', updated)
}
const emit = defineEmits(['edit', 'delete'])
</script>
