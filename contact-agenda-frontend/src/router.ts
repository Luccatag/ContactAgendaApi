import { createRouter, createWebHistory } from 'vue-router'

import ContactAgendaView from './views/ContactAgendaView.vue'
import AllContactsView from './views/AllContactsView.vue'
import AddContactView from './views/AddContactView.vue'

const routes = [
  { path: '/', component: ContactAgendaView },
  { path: '/all-contacts', component: AllContactsView },
  { path: '/add-contact', component: AddContactView }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
