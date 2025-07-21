import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import './style.css'
import './assets/theme.css'
import router from './router'

// Create Pinia instance for state management
const pinia = createPinia()

// Create and configure the Vue app
createApp(App)
  .use(pinia)    // Add Pinia for state management
  .use(router)   // Add Vue Router for navigation
  .mount('#app')