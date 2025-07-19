<template>
  <nav style="margin-bottom: 1rem;">
    <a href="#" @click.prevent="showAgenda = true">Contact Agenda</a>
  </nav>
  <ContactAgendaView v-if="showAgenda" />
</template>

<script setup lang="ts">
import { ref } from 'vue'
import ContactAgendaView from './views/ContactAgendaView.vue'
const showAgenda = ref(true)
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
