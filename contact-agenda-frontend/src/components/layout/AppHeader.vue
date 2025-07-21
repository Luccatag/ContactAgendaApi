<template>
  <header class="app-header">
    <div class="container">
      <h1 class="logo">Contact Agenda</h1>
      <nav class="navigation">
        <router-link to="/" class="nav-link">All Contacts</router-link>
        <router-link to="/add-contact" class="nav-link">Add Contact</router-link>
        <router-link to="/contact-agenda" class="nav-link">Original View</router-link>
      </nav>
    </div>
  </header>
</template>

<script setup>
// No props or logic needed for now
</script>

<style scoped>
.app-header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 1rem 0;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  width: 100%;
}

.container {
  width: 100%;
  max-width: 100%;
  margin: 0 auto;
  padding: 0 1rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  box-sizing: border-box;
}

.logo {
  margin: 0;
  font-size: 1.5rem;
  font-weight: bold;
}

.navigation {
  display: flex;
  gap: 1rem;
}

.nav-link {
  color: white;
  text-decoration: none;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  transition: background-color 0.3s ease;
}

.nav-link:hover,
.nav-link.router-link-active {
  background-color: rgba(255, 255, 255, 0.2);
}

@media (max-width: 768px) {
  .container {
    flex-direction: column;
    gap: 1rem;
  }
  
  .navigation {
    flex-wrap: wrap;
    justify-content: center;
  }
}
</style>
