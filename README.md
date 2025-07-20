
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
- Backend CRUD uses real database (EF Core)
- Swagger UI tested and working for all CRUD endpoints
- FluentValidation integrated and enforced in controller actions
- Dapper integration implemented and testable via dedicated controller
- Code organized and follows clean architecture principles

## 🚧 Next Steps
1. **Add unit/integration tests for backend**
   - Create a test project and add tests for Service, Repository, and Dapper endpoints.
2. **Expand frontend to full CRUD**
   - Implement update and delete in Vue view.
3. **Refactor frontend to use component-based structure**
   - Modularize UI and use vue-router for navigation.
4. **Use axios for all API calls in frontend**
   - Replace fetch with axios in Vue components.
5. **Implement Pinia for state management (optional)**
   - Use Pinia to manage contacts state in frontend.
6. **Add frontend test coverage**
   - Add unit and integration tests for Vue components.
7. **Bonus: Implement CQRS, authentication/security, RabbitMQ, Dockerfile**
   - Add advanced features as needed to demonstrate differentials.

## 📋 Full Requirements Reference
- .NET 6+ backend with clean architecture (Repositories, Services, Controllers, Interfaces, Dependency Injection)
- Entity Framework Core for data access
- AutoMapper for mapping between models and DTOs
- FluentValidation for input validation
- Swashbuckle/Swagger for API documentation
- Dapper integration (sample query)
- Unit/integration tests for backend
- Vue 3 frontend with Vite
- PrimeVue for UI components
- axios for API calls
- vue-router for navigation
- pinia for state management (optional)
- Component-based frontend structure
- Frontend test coverage
- Bonus: CQRS, authentication/security, RabbitMQ, Dockerfile
