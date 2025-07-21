/**
 * Integration tests for App component
 * 
 * These tests verify that the main App component correctly:
 * - Renders the overall application structure
 * - Integrates with router for navigation
 * - Manages global application state
 * - Handles component communication
 */

import { describe, it, expect, vi, beforeEach } from 'vitest'
import { mount } from '@vue/test-utils'
import { createRouter, createWebHistory } from 'vue-router'
import { createPinia, setActivePinia } from 'pinia'
import App from '../App.vue'

// Mock components to avoid complex dependencies
vi.mock('../components/layout/AppHeader.vue', () => ({
  default: {
    name: 'AppHeader',
    template: '<header data-testid="app-header">App Header</header>'
  }
}))

// Create a mock router
const routes = [
  { path: '/', component: { template: '<div data-testid="home-view">Home View</div>' } },
  { path: '/contacts', component: { template: '<div data-testid="contacts-view">Contacts View</div>' } }
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

describe('App', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
  })

  describe('Component Structure', () => {
    it('should render the app container', async () => {
      const wrapper = mount(App, {
        global: {
          plugins: [router]
        }
      })

      expect(wrapper.find('#app').exists()).toBe(true)
    })

    it('should render the AppHeader component', async () => {
      const wrapper = mount(App, {
        global: {
          plugins: [router]
        }
      })

      expect(wrapper.find('[data-testid="app-header"]').exists()).toBe(true)
    })

    it('should render the main content area', async () => {
      const wrapper = mount(App, {
        global: {
          plugins: [router]
        }
      })

      expect(wrapper.find('.main-content').exists()).toBe(true)
    })

    it('should render router-view for page content', async () => {
      const wrapper = mount(App, {
        global: {
          plugins: [router]
        }
      })

      expect(wrapper.find('main.main-content').exists()).toBe(true)
    })
  })

  describe('Layout Structure', () => {
    it('should have proper CSS classes for layout', async () => {
      const wrapper = mount(App, {
        global: {
          plugins: [router]
        }
      })

      const appContainer = wrapper.find('#app')
      expect(appContainer.exists()).toBe(true)

      const mainContent = wrapper.find('.main-content')
      expect(mainContent.exists()).toBe(true)
    })

    it('should maintain proper HTML structure', async () => {
      const wrapper = mount(App, {
        global: {
          plugins: [router]
        }
      })

      // Check that the structure is: div#app > header + main
      const appDiv = wrapper.find('#app')
      expect(appDiv.exists()).toBe(true)

      const header = wrapper.find('[data-testid="app-header"]')
      const main = wrapper.find('.main-content')
      
      expect(header.exists()).toBe(true)
      expect(main.exists()).toBe(true)
    })
  })

  describe('Router Integration', () => {
    it('should render home view by default', async () => {
      router.push('/')
      await router.isReady()

      const wrapper = mount(App, {
        global: {
          plugins: [router]
        }
      })

      // Wait for router to settle
      await wrapper.vm.$nextTick()

      expect(wrapper.find('[data-testid="home-view"]').exists()).toBe(true)
    })

    it('should render contacts view when navigating to /contacts', async () => {
      router.push('/contacts')
      await router.isReady()

      const wrapper = mount(App, {
        global: {
          plugins: [router]
        }
      })

      // Wait for router to settle
      await wrapper.vm.$nextTick()

      expect(wrapper.find('[data-testid="contacts-view"]').exists()).toBe(true)
    })

    it('should update view when route changes', async () => {
      router.push('/')
      await router.isReady()

      const wrapper = mount(App, {
        global: {
          plugins: [router]
        }
      })

      // Initially should show home view
      await wrapper.vm.$nextTick()
      expect(wrapper.find('[data-testid="home-view"]').exists()).toBe(true)

      // Navigate to contacts
      router.push('/contacts')
      await wrapper.vm.$nextTick()
      expect(wrapper.find('[data-testid="contacts-view"]').exists()).toBe(true)
    })
  })

  describe('Global Styling', () => {
    it('should apply global CSS styles', async () => {
      const wrapper = mount(App, {
        global: {
          plugins: [router]
        }
      })

      const appElement = wrapper.find('#app')
      
      // Check if the component has the necessary structure for CSS to apply
      expect(appElement.exists()).toBe(true)
      expect(wrapper.find('.main-content').exists()).toBe(true)
    })

    it('should have responsive layout structure', async () => {
      const wrapper = mount(App, {
        global: {
          plugins: [router]
        }
      })

      // Verify that the layout elements exist for responsive design
      const appContainer = wrapper.find('#app')
      const mainContent = wrapper.find('.main-content')

      expect(appContainer.exists()).toBe(true)
      expect(mainContent.exists()).toBe(true)
    })
  })

  describe('Component Integration', () => {
    it('should properly integrate with Pinia store', async () => {
      const wrapper = mount(App, {
        global: {
          plugins: [router]
        }
      })

      // The component should mount without errors when Pinia is available
      expect(wrapper.vm).toBeDefined()
    })

    it('should handle component lifecycle correctly', async () => {
      const wrapper = mount(App, {
        global: {
          plugins: [router]
        }
      })

      // Component should be mounted and ready
      expect(wrapper.vm).toBeDefined()
      expect(wrapper.html()).toContain('data-testid="app-header"')
    })
  })

  describe('Accessibility', () => {
    it('should have proper semantic HTML structure', async () => {
      const wrapper = mount(App, {
        global: {
          plugins: [router]
        }
      })

      // Should have a main element for accessibility
      const mainElement = wrapper.find('main')
      expect(mainElement.exists()).toBe(true)
      expect(mainElement.classes()).toContain('main-content')
    })

    it('should maintain proper document structure', async () => {
      const wrapper = mount(App, {
        global: {
          plugins: [router]
        }
      })

      // Check for proper landmark elements
      expect(wrapper.find('main').exists()).toBe(true)
      
      // Header should be present (even if mocked)
      expect(wrapper.find('[data-testid="app-header"]').exists()).toBe(true)
    })
  })

  describe('Error Handling', () => {
    it('should handle router errors gracefully', async () => {
      // Test with invalid route
      router.push('/invalid-route')
      await router.isReady()

      const wrapper = mount(App, {
        global: {
          plugins: [router]
        }
      })

      // Component should still render even with invalid route
      expect(wrapper.find('#app').exists()).toBe(true)
      expect(wrapper.find('.main-content').exists()).toBe(true)
    })

    it('should render without throwing errors', async () => {
      expect(() => {
        mount(App, {
          global: {
            plugins: [router]
          }
        })
      }).not.toThrow()
    })
  })
})
