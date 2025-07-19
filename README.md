# Contact Agenda Project - Next Steps & Requirements

## ✅ Completed Steps
- Initialized .NET 6+ backend project (ContactAgendaApi)
- Initialized Vue 3 frontend project (contact-agenda-frontend)
- Solution structure: Models, DTOs, Repositories, Services, Controllers, Interfaces
- Set up EF Core and created AppDbContext
- Scaffolded Contact model (Name, Email, Phone)
- Planned and implemented API routes (CRUD)
- Installed and configured EntityFramework, AutoMapper, FluentValidation, Swashbuckle.Swagger
- Installed and configured PrimeVue, axios, vue-router, pinia (frontend)
- Created basic Vue view for adding/listing contacts
- Backend CRUD now uses real database (EF Core)
- Swagger UI tested and working for all CRUD endpoints

## 🚧 Next Steps
1. **Integrate FluentValidation in controller actions**
   - Enforce validation rules in API endpoints and return validation errors.
2. **Implement Dapper integration**
   - Add a sample Dapper-based query in the repository layer.
3. **Add unit/integration tests for backend**
   - Create a test project and add tests for Service/Repository.
4. **Expand frontend to full CRUD**
   - Implement update and delete in Vue view.
5. **Refactor frontend to use component-based structure**
   - Modularize UI and use vue-router for navigation.
6. **Use axios for all API calls in frontend**
   - Replace fetch with axios in Vue components.
7. **Implement Pinia for state management (optional)**
   - Use Pinia to manage contacts state in frontend.
8. **Add frontend test coverage**
   - Add unit and integration tests for Vue components.
9. **Bonus: Implement CQRS, authentication/security, RabbitMQ, Dockerfile**
   - Add advanced features as needed.

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
