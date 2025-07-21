<template>
  <nav class="app-header">
    <span class="logo">
      <svg width="32" height="32" viewBox="0 0 24 24" fill="none">
        <circle cx="12" cy="12" r="10" fill="#42b883" />
        <text x="12" y="16" text-anchor="middle" font-size="12" fill="white" font-family="Arial">CA</text>
      </svg>
    </span>
    <button @click="goToContacts">All Contacts</button>
    <button @click="goToAddContact">Add Contact</button>
  </nav>
</template>

<script setup>
import { useRouter } from 'vue-router'
const router = useRouter()
function goToContacts() {
  router.push('/home')
}
function goToAddContact() {
  router.push('/add-contact')
}
</script>

<style scoped>
.app-header {
  display: flex;
  align-items: center;
  gap: 1rem;
  background: #f8f8f8;
  padding: 1rem 2rem;
  border-bottom: 1px solid #e0e0e0;
}
.logo {
  display: flex;
  align-items: center;
  margin-right: 1rem;
}
button {
  background: #42b883;
  color: white;
  border: none;
  border-radius: 4px;
  padding: 0.5rem 1rem;
  cursor: pointer;
  font-weight: 500;
  transition: background 0.2s;
}
button:hover {
  background: #369870;
}
</style>
