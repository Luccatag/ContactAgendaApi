import { createApp } from 'vue'
import App from './App.vue'

// PrimeVue core
import PrimeVue from 'primevue/config'
// PrimeVue theme (choose one, e.g., Lara Light)
import '@primevuelabs/lara-theme/theme/lara-light-blue.css'
// PrimeVue base styles
import 'primevue/resources/primevue.min.css'
import 'primeicons/primeicons.css'
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import './assets/theme.css'

const app = createApp(App)
app.use(PrimeVue)
app.component('InputText', InputText)
app.component('Button', Button)
app.component('DataTable', DataTable)
app.component('Column', Column)
app.mount('#app')
