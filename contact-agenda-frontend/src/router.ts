import { createRouter, createWebHistory } from 'vue-router'

import HomeView from './views/HomeView.vue'
import ContactAgendaView from './views/ContactAgendaView.vue'
import AllContactsView from './views/AllContactsView.vue'
import AddContactView from './views/AddContactView.vue'

const routes = [
  { path: '/', component: HomeView },
  { path: '/contacts', component: AllContactsView },
  { path: '/contact-agenda', component: ContactAgendaView },
  { path: '/add', component: AddContactView }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
