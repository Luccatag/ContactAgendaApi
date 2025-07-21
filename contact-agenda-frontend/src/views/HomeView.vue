<template>
  <div class="home-view">
    <!-- Header Section -->
    <div class="home-header">
      <h1 class="home-title">
        <span class="emoji">⭐</span>
        Favorite Contacts
      </h1>
      <p class="home-subtitle">Quick access to your most important contacts</p>
    </div>

    <!-- Loading State -->
    <div v-if="contactStore.loading" class="loading-state">
      <div class="loading-spinner"></div>
      <p>Loading your favorite contacts...</p>
    </div>

    <!-- Error State -->
    <div v-else-if="contactStore.error" class="error-state">
      <div class="error-icon">❌</div>
      <h3>Error Loading Contacts</h3>
      <p>{{ contactStore.error }}</p>
      <button @click="contactStore.fetchContacts(true)" class="btn btn-retry">
        Try Again
      </button>
    </div>

    <!-- No Favorites State -->
    <div v-else-if="favoriteContacts.length === 0" class="empty-state">
      <div class="empty-icon">💫</div>
      <h3>No Favorite Contacts Yet</h3>
      <p class="empty-message">
        Start by marking some contacts as favorites using the heart button (🤍) on any contact card.
      </p>
      <div class="empty-actions">
        <router-link to="/contacts" class="btn btn-primary">
          View All Contacts
        </router-link>
        <router-link to="/add" class="btn btn-secondary">
          Add New Contact
        </router-link>
      </div>
    </div>

    <!-- Favorites Grid -->
    <div v-else class="favorites-section">
      <div class="favorites-stats">
        <span class="stats-text">
          {{ favoriteContacts.length }} favorite contact{{ favoriteContacts.length !== 1 ? 's' : '' }}
        </span>
        <router-link to="/contacts" class="view-all-link">
          View all contacts →
        </router-link>
      </div>

      <div class="favorites-grid">
        <ContactCard
          v-for="contact in favoriteContacts"
          :key="contact.id"
          :contact="contact"
          @updated="handleContactUpdated"
          @deleted="handleContactDeleted"
        />
      </div>

      <!-- Quick Actions -->
      <div class="quick-actions">
        <h3 class="quick-actions-title">Quick Actions</h3>
        <div class="quick-actions-grid">
          <router-link to="/add" class="action-card">
            <div class="action-icon">➕</div>
            <h4>Add Contact</h4>
            <p>Create a new contact</p>
          </router-link>
          
          <router-link to="/contacts" class="action-card">
            <div class="action-icon">📋</div>
            <h4>All Contacts</h4>
            <p>Browse all contacts</p>
          </router-link>
          
          <button @click="refreshContacts" class="action-card action-button">
            <div class="action-icon">🔄</div>
            <h4>Refresh</h4>
            <p>Update contact list</p>
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, onMounted } from 'vue'
import { useContactStore } from '../stores/contactStore'
import ContactCard from '../components/ui/ContactCard.vue'

// Initialize the contact store
const contactStore = useContactStore()

/**
 * Computed property to get only favorite contacts
 * Automatically updates when contacts change
 */
const favoriteContacts = computed(() => {
  return contactStore.sortedContacts.filter(contact => contact.isFavorite)
})

/**
 * Load contacts when component mounts
 * Only fetches if not already loaded to avoid unnecessary API calls
 */
onMounted(async () => {
  if (!contactStore.isInitialized) {
    await contactStore.fetchContacts()
  }
})

/**
 * Handle contact updates from ContactCard component
 * The store automatically handles the update, but we can add additional logic here if needed
 */
const handleContactUpdated = (updatedContact) => {
  console.log('📝 Contact updated on home page:', updatedContact.name)
  // Store automatically updates, no additional action needed
}

/**
 * Handle contact deletion from ContactCard component
 * The store automatically handles the deletion, but we can add additional logic here if needed
 */
const handleContactDeleted = (contactId) => {
  console.log('🗑️ Contact deleted from home page:', contactId)
  // Store automatically updates, no additional action needed
}

/**
 * Manually refresh contacts
 * Forces a fresh fetch from the API
 */
const refreshContacts = async () => {
  await contactStore.fetchContacts(true)
}
</script>

<style scoped>
/* Main container */
.home-view {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

/* Header styles */
.home-header {
  text-align: center;
  margin-bottom: 3rem;
}

.home-title {
  font-size: 2.5rem;
  font-weight: 700;
  color: #2d3748;
  margin: 0 0 0.5rem 0;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
}

.emoji {
  font-size: 2rem;
}

.home-subtitle {
  font-size: 1.125rem;
  color: #718096;
  margin: 0;
}

/* Loading state */
.loading-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 4rem 0;
  color: #718096;
}

.loading-spinner {
  width: 40px;
  height: 40px;
  border: 4px solid #e2e8f0;
  border-left: 4px solid #667eea;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin-bottom: 1rem;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

/* Error state */
.error-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 4rem 0;
  text-align: center;
}

.error-icon {
  font-size: 3rem;
  margin-bottom: 1rem;
}

.error-state h3 {
  color: #e53e3e;
  margin: 0 0 0.5rem 0;
}

.error-state p {
  color: #718096;
  margin: 0 0 1.5rem 0;
}

/* Empty state */
.empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 4rem 0;
  text-align: center;
}

.empty-icon {
  font-size: 4rem;
  margin-bottom: 1.5rem;
}

.empty-state h3 {
  color: #2d3748;
  margin: 0 0 1rem 0;
  font-size: 1.5rem;
}

.empty-message {
  color: #718096;
  margin: 0 0 2rem 0;
  max-width: 500px;
  line-height: 1.6;
}

.empty-actions {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
  justify-content: center;
}

/* Favorites section */
.favorites-section {
  margin-top: 2rem;
}

.favorites-stats {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  padding-bottom: 1rem;
  border-bottom: 1px solid #e2e8f0;
}

.stats-text {
  font-weight: 600;
  color: #2d3748;
  font-size: 1.125rem;
}

.view-all-link {
  color: #667eea;
  text-decoration: none;
  font-weight: 500;
  transition: color 0.2s ease;
}

.view-all-link:hover {
  color: #5a67d8;
}

/* Favorites grid */
.favorites-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
  gap: 1.5rem;
  margin-bottom: 3rem;
}

/* Quick actions */
.quick-actions {
  margin-top: 3rem;
  padding-top: 2rem;
  border-top: 1px solid #e2e8f0;
}

.quick-actions-title {
  color: #2d3748;
  margin: 0 0 1.5rem 0;
  font-size: 1.25rem;
  font-weight: 600;
}

.quick-actions-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
}

.action-card {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 1.5rem;
  background: white;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  text-decoration: none;
  color: inherit;
  transition: all 0.2s ease;
  cursor: pointer;
}

.action-button {
  border: 1px solid #e2e8f0;
  background: white;
  font: inherit;
}

.action-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  border-color: #cbd5e0;
}

.action-icon {
  font-size: 2rem;
  margin-bottom: 0.75rem;
}

.action-card h4 {
  margin: 0 0 0.5rem 0;
  color: #2d3748;
  font-size: 1rem;
  font-weight: 600;
}

.action-card p {
  margin: 0;
  color: #718096;
  font-size: 0.875rem;
  text-align: center;
}

/* Button styles */
.btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 6px;
  font-size: 1rem;
  font-weight: 500;
  text-decoration: none;
  cursor: pointer;
  transition: all 0.2s ease;
  min-width: 120px;
}

.btn-primary {
  background: #667eea;
  color: white;
}

.btn-primary:hover {
  background: #5a67d8;
}

.btn-secondary {
  background: #e2e8f0;
  color: #4a5568;
}

.btn-secondary:hover {
  background: #cbd5e0;
}

.btn-retry {
  background: #38a169;
  color: white;
}

.btn-retry:hover {
  background: #2f855a;
}

/* Responsive design */
@media (max-width: 768px) {
  .home-view {
    padding: 1rem;
  }
  
  .home-title {
    font-size: 2rem;
  }
  
  .favorites-grid {
    grid-template-columns: 1fr;
  }
  
  .favorites-stats {
    flex-direction: column;
    gap: 1rem;
    text-align: center;
  }
  
  .empty-actions {
    flex-direction: column;
    align-items: center;
  }
  
  .quick-actions-grid {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 480px) {
  .home-header {
    margin-bottom: 2rem;
  }
  
  .home-title {
    font-size: 1.75rem;
    flex-direction: column;
    gap: 0.25rem;
  }
  
  .favorites-grid {
    gap: 1rem;
  }
}
</style>
