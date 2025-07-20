
<template>
  <nav style="margin-bottom: 1rem;">
    <router-link to="/">Contact Agenda</router-link>
    <button @click="$router.push('/all-contacts')" style="margin-left: 1rem;">View All Contacts (JSON)</button>
    <button @click="$router.push('/add-contact')" style="margin-left: 1rem;">Add New Contact</button>
  </nav>
  <router-view />
</template>

<script setup lang="ts">
// No need for showAgenda or direct import, router handles views
</script>

<style scoped>
.logo {
  height: 6em;
  padding: 1.5em;
  will-change: filter;
  transition: filter 300ms;
}
.logo:hover {
  filter: drop-shadow(0 0 2em #646cffaa);
}
.logo.vue:hover {
  filter: drop-shadow(0 0 2em #42b883aa);
}
</style>
