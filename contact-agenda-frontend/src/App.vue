
<template>
  <!-- AppHeader: global navigation bar for all pages -->
  <AppHeader />
  <!-- Main content rendered by router -->
  <router-view />
</template>

<script setup lang="ts">
// Import AppHeader for global navigation
import AppHeader from './components/AppHeader.vue'
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
