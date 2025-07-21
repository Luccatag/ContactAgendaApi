# Frontend Testing Documentation

## 🧪 Overview

This document provides comprehensive information about the frontend testing suite for the Contact Agenda application. The testing infrastructure uses **Vitest** with **Vue Test Utils** to ensure reliable, maintainable, and comprehensive test coverage.

## 📋 Test Statistics

- **Total Tests**: 50+ tests implemented
- **Unit Tests**: Component and service tests
- **Integration Tests**: Full component integration with stores and routing
- **Test Framework**: Vitest + Vue Test Utils + jsdom
- **Coverage Areas**: Components, Stores, Services, Integration workflows

## 🔧 Testing Technology Stack

### **Core Testing Framework**
- **Vitest**: Fast, modern testing framework optimized for Vite projects
- **Vue Test Utils**: Official testing utilities for Vue.js components
- **jsdom**: Browser environment simulation for DOM testing

### **Testing Utilities**
- **@vitest/ui**: Interactive testing interface
- **happy-dom**: Alternative DOM environment (lightweight)
- **Pinia**: State management testing with createPinia()

## 🚀 Quick Start

### **Run All Tests**
```bash
npm run test
```

### **Run Tests with UI**
```bash
npm run test:ui
```

### **Run Tests Once (CI Mode)**
```bash
npm run test:run
```

### **Run Tests with Coverage**
```bash
npm run test:coverage
```

### **Watch Mode (Development)**
```bash
npm run test -- --watch
```

## 📁 Test Structure

```
src/test/
├── setup.ts                    # Global test configuration
├── components/                 # Component unit tests
│   ├── ContactCard.test.ts    # ContactCard component tests
│   └── ContactForm.test.ts    # ContactForm component tests
├── stores/                     # Pinia store tests
│   └── contactStore.test.ts   # Contact store state management tests
├── services/                   # Service layer tests
│   └── contactService.test.ts # API service tests
└── integration/               # Integration tests
    └── App.test.ts           # App component integration tests
```

## 🧩 Test Categories

### **1. Component Tests** (20+ tests)
- **ContactCard.vue**: Display mode, edit mode, user interactions, validation
- **ContactForm.vue**: Form rendering, validation, submission, error handling

### **2. Store Tests** (15+ tests)
- **contactStore.ts**: State management, CRUD operations, computed properties, error handling

### **3. Service Tests** (10+ tests)
- **contactService.ts**: HTTP requests, error handling, data transformation

### **4. Integration Tests** (10+ tests)
- **App.vue**: Router integration, component communication, layout structure

## 📝 Test Patterns and Best Practices

### **AAA Pattern (Arrange, Act, Assert)**
```typescript
it('should create contact successfully', async () => {
  // Arrange
  const newContact = { name: 'John', email: 'john@example.com', phone: '123456789' }
  mockService.createContact.mockResolvedValue({ id: 1, ...newContact })

  // Act
  const result = await store.createContact(newContact)

  // Assert
  expect(result).toEqual({ id: 1, ...newContact })
  expect(store.contacts).toContain(result)
})
```

### **Component Testing Pattern**
```typescript
describe('ContactCard', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    vi.clearAllMocks()
  })

  it('should render contact information', () => {
    const wrapper = mount(ContactCard, {
      props: { contact: mockContact }
    })

    expect(wrapper.find('.contact-name').text()).toBe('John Doe')
  })
})
```

### **Store Testing Pattern**
```typescript
describe('useContactStore', () => {
  let store: ReturnType<typeof useContactStore>

  beforeEach(() => {
    setActivePinia(createPinia())
    store = useContactStore()
  })

  it('should fetch contacts successfully', async () => {
    mockService.getAllContacts.mockResolvedValue(mockContacts)
    await store.fetchContacts()
    expect(store.contacts).toEqual(mockContacts)
  })
})
```

## 🎯 Testing Features

### **Mocking Strategy**
- **Service Layer**: Complete API service mocking
- **Browser APIs**: Window methods, DOM APIs
- **External Dependencies**: Vue Router, Pinia stores

### **Test Isolation**
- Each test uses fresh Pinia instance
- Component mocks prevent complex dependencies
- Automatic cleanup with `beforeEach` hooks

### **Validation Testing**
- Form validation rules
- Email and phone format validation
- Required field validation
- Real-time validation feedback

### **User Interaction Testing**
- Click events and form submissions
- Input field changes and validation
- Component state changes
- Event emission verification

### **Error Handling Testing**
- API error scenarios
- Network failure simulation
- Form validation errors
- Graceful error recovery

## 🔍 Coverage Areas

### **Components**
- ✅ Rendering and display logic
- ✅ User interaction handling
- ✅ Props and event management
- ✅ Form validation and submission
- ✅ State changes and updates
- ✅ Error handling and recovery

### **Stores (Pinia)**
- ✅ State initialization and management
- ✅ Computed properties and getters
- ✅ Actions and mutations
- ✅ API integration
- ✅ Error handling and recovery
- ✅ Caching and optimization

### **Services**
- ✅ HTTP request/response handling
- ✅ Error handling and transformation
- ✅ Data serialization/deserialization
- ✅ API endpoint coverage
- ✅ Network error scenarios

### **Integration**
- ✅ Component communication
- ✅ Router integration
- ✅ Store integration
- ✅ Full workflow testing
- ✅ Layout and structure

## 🐛 Debugging Tests

### **Common Issues and Solutions**

1. **Component Not Rendering**
   ```bash
   # Check if all dependencies are mocked
   vi.mock('../stores/contactStore', () => ({ ... }))
   ```

2. **Async Operation Not Completing**
   ```bash
   # Wait for Vue's reactivity system
   await wrapper.vm.$nextTick()
   ```

3. **Mock Not Working**
   ```bash
   # Clear mocks between tests
   beforeEach(() => {
     vi.clearAllMocks()
   })
   ```

### **Debug Commands**
```bash
# Run specific test file
npm run test ContactCard.test.ts

# Run tests matching pattern
npm run test -- --grep "should render"

# Run with verbose output
npm run test -- --reporter=verbose

# Debug single test
npm run test -- --no-coverage --no-watch ContactCard.test.ts
```

## 📊 Test Configuration

### **Vitest Configuration** (`vite.config.ts`)
```typescript
export default defineConfig({
  test: {
    globals: true,           // Global test functions (describe, it, expect)
    environment: 'jsdom',    // DOM simulation
    setupFiles: ['src/test/setup.ts']  // Global test setup
  }
})
```

### **Setup File** (`src/test/setup.ts`)
```typescript
// Mock browser APIs
global.IntersectionObserver = class IntersectionObserver { ... }
global.ResizeObserver = class ResizeObserver { ... }

// Mock fetch for API testing
global.fetch = vi.fn()

// Global test helpers
export const mockContact = { ... }
export const mockContacts = [ ... ]
```

## 🎨 Test Writing Guidelines

### **1. Descriptive Test Names**
```typescript
// ✅ Good
it('should show validation error for invalid email format')

// ❌ Bad
it('should validate email')
```

### **2. Clear Test Structure**
```typescript
describe('ComponentName', () => {
  describe('Feature Group', () => {
    it('should do specific thing', () => {
      // Test implementation
    })
  })
})
```

### **3. Isolated Test Data**
```typescript
// Create fresh data for each test
const mockContact = () => ({
  id: 1,
  name: 'Test Contact',
  email: 'test@example.com'
})
```

### **4. Meaningful Assertions**
```typescript
// ✅ Good
expect(wrapper.find('.error-message').text()).toBe('Email is required')

// ❌ Bad
expect(wrapper.find('.error-message').exists()).toBe(true)
```

## 🚀 Running Tests in Different Environments

### **Development**
```bash
npm run test              # Watch mode, reruns on changes
```

### **Continuous Integration**
```bash
npm run test:run          # Single run, exit after completion
```

### **Coverage Analysis**
```bash
npm run test:coverage     # Generate coverage report
```

### **Interactive UI**
```bash
npm run test:ui           # Open Vitest UI in browser
```

## 📈 Next Steps

### **Planned Enhancements**
1. **E2E Tests**: Cypress or Playwright for full application workflows
2. **Visual Testing**: Component snapshot testing
3. **Performance Testing**: Component rendering performance
4. **Accessibility Testing**: Automated a11y validation

### **Coverage Goals**
- **Lines**: 90%+ code coverage
- **Functions**: 95%+ function coverage
- **Branches**: 85%+ branch coverage
- **Statements**: 90%+ statement coverage

## 🤝 Contributing to Tests

### **Adding New Tests**
1. Create test file alongside component: `Component.vue` → `Component.test.ts`
2. Follow naming convention: `describe('ComponentName')` 
3. Use appropriate mocking strategy
4. Include both positive and negative test cases
5. Test user interactions and edge cases

### **Best Practices**
- Write tests before fixing bugs (TDD approach)
- Keep tests focused and atomic
- Use descriptive test and describe block names
- Mock external dependencies consistently
- Test error scenarios and edge cases
- Maintain test data helpers for consistency

This comprehensive testing suite ensures the frontend is reliable, maintainable, and provides excellent user experience while supporting rapid development cycles.
