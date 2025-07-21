<template>
  <div class="contact-list-grid">
    <ContactItem
      v-for="contact in contacts"
      :key="contact.id"
      :contact="contact"
      @edit="onEdit"
      @delete="onDelete"
    />
  </div>
</template>

<script setup>
// ContactList.vue: Renders a responsive grid of contacts using ContactItem
// Props: contacts (array), emits: edit, delete
import ContactItem from './ContactItem.vue'
import { defineProps, defineEmits } from 'vue'
const props = defineProps({ contacts: Array })
const emit = defineEmits(['edit', 'delete'])
function onEdit(contact) { emit('edit', contact) }
function onDelete(id) { emit('delete', id) }
</script>

<style scoped>
/* Responsive grid using full screen width */
/* Responsive grid limited to container width */


.contact-list-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
  gap: 1.5rem;
  padding: 1rem;
  width: 100%;
  box-sizing: border-box;
}

@media (max-width: 600px) {
  .contact-list-grid {
    grid-template-columns: 1fr;
    padding: 0.5rem;
    gap: 1rem;
  }
}
</style>
