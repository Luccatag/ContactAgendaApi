
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

## 🚀 Quick Start

### Prerequisites
- Docker Desktop installed and running
- Git (to clone the repository)

### Start the Application
```bash
# Clone the repository
git clone https://github.com/Luccatag/ContactAgendaApi.git
cd ContactAgendaApi

# Quick start (Linux/macOS)
./start.sh

# Quick start (Windows)
.\start.ps1

# Or use Docker Compose directly
docker-compose up -d
```

### Access Your Application
- **Frontend**: http://localhost:3000
- **API**: http://localhost:8081
- **Swagger Documentation**: http://localhost:8081/swagger

### Useful Commands
```bash
# Check status
docker-compose ps

# View logs
docker-compose logs -f

# Stop application
docker-compose down
```

For detailed Docker instructions, see [DOCKER-RUN-GUIDE.md](DOCKER-RUN-GUIDE.md)

---

## ✅ Project Status & Completed Features

### **Database and Data Access**
- EF Core set up with AppDbContext and SQLite database ✅
- Contact model scaffolded (Name, Email, Phone, IsFavorite) ✅
- **SQLite database migration completed** ✅
- Production application now uses SQLite for data persistence ✅
- Repository pattern supports multiple implementations (EF Core, JSON, Dapper) ✅
- Database migrations created and applied successfully ✅

### **API Development**
- API routes (CRUD) planned and implemented ✅
- **API endpoints using SQLite database**: `/api/contacts` for production persistence ✅
- Swagger UI tested and working for all CRUD endpoints ✅
- FluentValidation integrated and enforced in controller actions ✅
- All validation rules properly functioning (empty fields, invalid email format) ✅

### **Frontend Infrastructure Setup**
- Vue 3 frontend project initialized (contact-agenda-frontend)
- PrimeVue, axios, vue-router, pinia configured in frontend
- Navigation between pages using vue-router

### **Frontend CRUD Implementation**
- Basic Vue view for adding/listing contacts created
- Frontend CRUD (add, edit, view) implemented and connected to backend
- Currently using JSON file persistence as temporary database for demonstration

### **Frontend refactored to use component-based structure**
- Created modular components: AppHeader, ContactCard, ContactForm
- Organized components into layout/ and ui/ folders
- Implemented reusable ContactCard with inline editing and validation
- Added comprehensive code comments and documentation
- Responsive design for mobile and desktop

### **Email and phone validation implemented**
- Client-side validation with regex patterns
- Server-side validation before API calls
- User-friendly error messages and real-time validation

### **Axios integration completed**
- Replaced all fetch calls with axios for consistency
- Centralized API service with proper error handling
- Enhanced error messages and timeout configuration
- Type-safe API calls with TypeScript support
- Fixed API endpoints to use correct JSON persistence routes (/api/contacts)

### **Pinia state management implemented**
- Centralized contact state management across all components
- Intelligent caching to prevent unnecessary API calls
- Optimistic updates for immediate UI feedback
- Real-time search functionality with computed properties
- Duplicate email detection before contact creation
- Automatic error handling and recovery mechanisms

### **Favorite contacts functionality implemented**
- Added IsFavorite field to Contact model and database schema
- Heart button (❤️/🤍) for each contact card to toggle favorite status
- Favorites automatically displayed at top of contact list
- Smooth animations and visual feedback for favorite interactions
- Favorite status preserved in both edit forms and new contact creation
- Backend API endpoint `/api/contacts/{id}/favorite` for toggle operations

### **Testing Infrastructure Completed**
- **Backend Tests**: 54/54 passing ✅ (42 unit + 12 integration tests)
- **Frontend Tests**: 39/39 passing ✅ (services, stores, components, validation)
- Complete testing infrastructure with xUnit, Moq, FluentAssertions (backend)
- Vitest + Vue Test Utils + TypeScript integration (frontend)
- Comprehensive test documentation and README files

### **CQRS Design Pattern Implementation**
- Complete CQRS folder structure (`/CQRS/Commands`, `/CQRS/Queries`, `/CQRS/Handlers`)
- MediatR package integration (v13.0.0)
- All CRUD commands and queries with dedicated handlers
- CQRS controller at `/api/v2/ContactsCqrsSimple`
- Comprehensive logging and type safety
- Performance optimized read/write operations

### **Docker Infrastructure**
- Multi-stage Docker builds for backend and frontend
- Multiple Docker Compose configurations (fullstack, simple, database, mysql)
- Container orchestration with health checks and persistence
- Quick-start scripts for all platforms (Windows/Linux/macOS)
- Organized docker folder structure with comprehensive documentation

---

## 🚧 Next Steps & Planned Features
## 🚧 Next Steps & Planned Features

### **1. Authentication & Security** 🚧 **NEXT PRIORITY**
- JWT Authentication for API endpoints
- Role-based authorization (Admin, User roles)
- API key authentication for external integrations
- Rate limiting and throttling
- User registration and login endpoints
- Protected contact data per user

### **2. RabbitMQ Messaging** 🚧 **PLANNED**
- Event-driven architecture with contact events
- Async processing for notifications
- Message queuing for scalable operations
- Integration with CQRS events
- Background service for message consumption
- Retry and dead letter queue handling

### **3. Enhanced Features** 🚧 **FUTURE**
- Contact import/export functionality
- Advanced search and filtering
- Contact groups and categories
- Bulk operations
- Email integration
- Contact sharing between users

---

## 📋 Requirements & Implementation Status

### **Core Requirements** ✅ **ALL COMPLETED**
- .NET 6+ backend with clean architecture (Repositories, Services, Controllers, Interfaces, DI) ✅
- Entity Framework Core for data access ✅
- AutoMapper for mapping between models and DTOs ✅
- FluentValidation for input validation ✅
- Swashbuckle/Swagger for API documentation ✅
- Dapper integration (sample query) ✅
- Backend test coverage ✅ (54/54 tests passing)
- Vue 3 frontend with Vite ✅
- PrimeVue for UI components ✅
- axios for API calls ✅
- vue-router for navigation ✅
- pinia for state management ✅
- Component-based frontend structure ✅
- Frontend test coverage ✅ (39/39 tests passing)

### **Bonus Features (Differentials)**
- **CQRS Design Pattern** ✅ **COMPLETED** - Full implementation with MediatR
- **Application Dockerfile** ✅ **COMPLETED** - Multi-container setup with Docker Compose
- **Authentication/Security** ❌ **PLANNED** - JWT, role-based auth
- **Messaging with RabbitMQ** ❌ **PLANNED** - Event-driven architecture

### **🎉 Test Coverage Summary**
- **Total Tests**: 93/93 passing ✅
- **Backend**: 54 tests (42 unit + 12 integration)
- **Frontend**: 39 tests (services, stores, components)

---

## � Technical Architecture
### API Endpoints
- **`/api/contacts`**: Main CRUD endpoints using **SQLite database** (production persistence)
  - `GET /api/contacts` - Retrieve all contacts (favorites listed first)
  - `GET /api/contacts/{id}` - Retrieve specific contact
  - `POST /api/contacts` - Create new contact (with validation)
  - `PUT /api/contacts/{id}` - Update existing contact
  - `DELETE /api/contacts/{id}` - Delete contact
  - `PATCH /api/contacts/{id}/favorite` - Toggle favorite status
- **`/api/contactagenda`**: Demo endpoints using in-memory storage
- **`/api/dappercontacts`**: Sample Dapper integration endpoint

### Database Strategy
- **Production**: **SQLite database** (`contacts.db`) with EF Core for all data persistence ✅
- **Development**: Repository pattern allows switching between storage implementations
- **Testing**: Integration tests use isolated SQLite test databases for proper test isolation
- **Schema**: Contact model includes Id, Name, Email, Phone, and IsFavorite fields
- **Migrations**: Database migrations properly applied with IsFavorite field support

### Frontend Architecture
- **Component Structure**: Organized into `layout/` and `ui/` folders
- **State Management**: Pinia store with centralized contact data, optimistic updates, and intelligent caching
- **API Layer**: Axios-based service layer with centralized error handling
- **Responsive Design**: CSS Grid with automatic column adjustment based on screen width
- **Validation**: Client-side regex validation with server-side FluentValidation backup
- **Real-time Features**: Search filtering and favorite system

---

## 🧪 Testing

### **Comprehensive Test Coverage**
- **Total Tests**: 93/93 passing ✅
- **Backend**: 54 tests (42 unit + 12 integration) using xUnit, Moq, FluentAssertions
- **Frontend**: 39 tests (services, stores, components) using Vitest + Vue Test Utils

### **Running Tests**
```bash
# Backend tests
dotnet test

# Frontend tests  
cd contact-agenda-frontend && npx vitest run
```

For detailed testing documentation, see:
- [ContactAgendaApi.Tests/README.md](ContactAgendaApi.Tests/README.md)
- [contact-agenda-frontend/FRONTEND-TESTS-SUMMARY.md](contact-agenda-frontend/FRONTEND-TESTS-SUMMARY.md)

---

## 📚 Additional Documentation

- **[DOCKER-RUN-GUIDE.md](DOCKER-RUN-GUIDE.md)** - Complete Docker setup and usage guide
- **[REPOSITORY-CLEANUP-SUMMARY.md](REPOSITORY-CLEANUP-SUMMARY.md)** - Repository organization details
- **[CQRS/](CQRS/)** - CQRS implementation documentation
- **[ContactAgendaApi.Tests/](ContactAgendaApi.Tests/)** - Backend testing documentation
- **[contact-agenda-frontend/](contact-agenda-frontend/)** - Frontend project and tests
