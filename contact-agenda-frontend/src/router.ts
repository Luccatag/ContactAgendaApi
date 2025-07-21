import { createRouter, createWebHistory } from 'vue-router'

// Removed unused ContactAgendaView import
import AllContactsView from './views/AllContactsView.vue'
import AddContactView from './views/AddContactView.vue'

const routes = [
  { path: '/home', component: AllContactsView },
  { path: '/', redirect: '/home' },
  { path: '/add-contact', component: AddContactView }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
