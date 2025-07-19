<template>
  <div class="contacts-view">
    <h2>Contact Agenda</h2>
    <form @submit.prevent="addContact">
      <div>
        <label>Name:</label>
        <InputText v-model="newContact.name" required />
      </div>
      <div>
        <label>Email:</label>
        <InputText v-model="newContact.email" required />
      </div>
      <div>
        <label>Phone:</label>
        <InputText v-model="newContact.phone" required />
      </div>
      <Button label="Add Contact" type="submit" />
    </form>

    <DataTable :value="contacts" class="p-mt-4">
      <Column field="name" header="Name" />
      <Column field="email" header="Email" />
      <Column field="phone" header="Phone" />
    </DataTable>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

interface Contact {
  name: string;
  email: string;
  phone: string;
}

const contacts = ref<Contact[]>([]);
const newContact = ref({
  name: '',
  email: '',
  phone: ''
});

function addContact() {
  if (newContact.value.name && newContact.value.email && newContact.value.phone) {
    contacts.value.push({ ...newContact.value });
    newContact.value.name = '';
    newContact.value.email = '';
    newContact.value.phone = '';
  }
}
</script>

