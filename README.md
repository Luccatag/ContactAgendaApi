
# Contact Agenda Project

## 📄 Challenge Description (Translated)

You are to develop a CRUD application for a contact agenda (name, email, and phone) using .NET 6 or higher for the backend and Vue.js for the frontend.

**Requirements:**
- Business rules and validations (you define the rules)
- Use of design patterns (repositories, services, controllers, interfaces, dependency injection, etc.)
- Use of Entity Framework
- Use of accessory libraries/frameworks (e.g., Dapper, AutoMapper, FluentValidation)
- Use of Swagger
- Clean and organized code
- Backend test coverage
- Strong use of components in the frontend

**Differentials:**
- Use of CQRS design pattern
- Authentication/Security in the backend
- Messaging with RabbitMQ
- Frontend test coverage
- Application Dockerfile

The project itself is simple, but you are encouraged to enhance it to best showcase your skills as a developer. You may use any type of database.

## ✅ Completed Steps
- .NET 6+ backend project initialized (ContactAgendaApi)
- Vue 3 frontend project initialized (contact-agenda-frontend)
- Solution structured with Models, DTOs, Repositories, Services, Controllers, Interfaces
- EF Core set up with AppDbContext and SQLite database
- Contact model scaffolded (Name, Email, Phone)
- API routes (CRUD) planned and implemented
- EntityFramework, AutoMapper, FluentValidation, Swashbuckle/Swagger configured
- PrimeVue, axios, vue-router, pinia configured in frontend
- Basic Vue view for adding/listing contacts created
- Frontend CRUD (add, edit, view) implemented and connected to backend (JSON persistence)
- Navigation between pages using vue-router
- Backend CRUD supports both real database (EF Core) and JSON file persistence (via repository pattern)
- Swagger UI tested and working for all CRUD endpoints
- FluentValidation integrated and enforced in controller actions
- Dapper integration implemented and testable via dedicated controller
- Code organized and follows clean architecture principles
- **Frontend refactored to use component-based structure**
  - Created modular components: AppHeader, ContactCard, ContactForm
  - Organized components into layout/ and ui/ folders
  - Implemented reusable ContactCard with inline editing and validation
  - Added comprehensive code comments and documentation
  - Responsive design for mobile and desktop
- **Email and phone validation implemented**
  - Client-side validation with regex patterns
  - Server-side validation before API calls
  - User-friendly error messages and real-time validation
- **Axios integration completed**
  - Replaced all fetch calls with axios for consistency
  - Centralized API service with proper error handling
  - Enhanced error messages and timeout configuration
  - Type-safe API calls with TypeScript support

## 🚧 Next Steps
1. **Add unit/integration tests for backend**
   - Create a test project and add tests for Service, Repository (including JSON persistence), and Dapper endpoints.
2. ~~**Refactor frontend to use component-based structure**~~ ✅ **COMPLETED**
   - ~~Modularize UI and views for maintainability.~~
3. ~~**Use axios for all API calls in frontend**~~ ✅ **COMPLETED**
   - ~~Replace fetch with axios in Vue components for consistency and better error handling.~~
4. **Implement Pinia for state management (optional)**
   - Use Pinia to manage contacts state in frontend.
5. **Add frontend test coverage**
   - Add unit and integration tests for Vue components.
6. **Bonus: Implement CQRS, authentication/security, RabbitMQ, Dockerfile**
   - Add advanced features as needed to demonstrate differentials.

## 📋 Full Requirements Reference
- .NET 6+ backend with clean architecture (Repositories, Services, Controllers, Interfaces, Dependency Injection) ✅
- Entity Framework Core for data access ✅
- AutoMapper for mapping between models and DTOs ✅
- FluentValidation for input validation ✅
- Swashbuckle/Swagger for API documentation ✅
- Dapper integration (sample query) ✅
- Unit/integration tests for backend ❌
- Vue 3 frontend with Vite ✅
- PrimeVue for UI components ✅
- axios for API calls ✅
- vue-router for navigation ✅
- pinia for state management (optional) ✅ (configured but not actively used)
- Component-based frontend structure ✅
- Frontend test coverage ❌
- Bonus: CQRS, authentication/security, RabbitMQ, Dockerfile ❌
