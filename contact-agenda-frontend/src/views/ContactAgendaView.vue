<template>
  <div class="contact-agenda">
    <h2>Contact Agenda</h2>
    <form @submit.prevent="addContact">
      <input v-model="newContact.name" placeholder="Name" required />
      <input v-model="newContact.email" placeholder="Email" required />
      <input v-model="newContact.phone" placeholder="Phone" required />
      <button type="submit">Add Contact</button>
    </form>
    <ul>
      <li v-for="contact in contacts" :key="contact.id">
        {{ contact.name }} - {{ contact.email }} - {{ contact.phone }}
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'

interface Contact {
  id: number
  name: string
  email: string
  phone: string
}

const contacts = ref<Contact[]>([])
const newContact = ref({ name: '', email: '', phone: '' })

async function fetchContacts() {
  const res = await fetch('http://localhost:5000/api/contactagenda')
  contacts.value = await res.json()
}

async function addContact() {
  await fetch('http://localhost:5000/api/contactagenda', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(newContact.value)
  })
  newContact.value = { name: '', email: '', phone: '' }
  fetchContacts()
}

onMounted(fetchContacts)
</script>

<style scoped>
.contact-agenda { max-width: 400px; margin: 2rem auto; }
form { display: flex; flex-direction: column; gap: 0.5rem; }
ul { margin-top: 1rem; }
</style>
