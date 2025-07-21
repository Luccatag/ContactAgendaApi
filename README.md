
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
- **Backend Infrastructure Setup**
  - .NET 6+ backend project initialized (ContactAgendaApi)
  - Solution structured with Models, DTOs, Repositories, Services, Controllers, Interfaces
  - Code organized and follows clean architecture principles
  - EntityFramework, AutoMapper, FluentValidation, Swashbuckle/Swagger configured

- **Database and Data Access**
  - EF Core set up with AppDbContext and SQLite database
  - Contact model scaffolded (Name, Email, Phone)
  - Backend CRUD supports both real database (EF Core) and JSON file persistence (via repository pattern)
  - JSON file (`contacts.json`) used as temporary database substitute for demonstration
  - Dapper integration implemented and testable via dedicated controller

- **API Development**
  - API routes (CRUD) planned and implemented
  - **API endpoints properly configured**: `/api/contacts` for JSON persistence, `/api/contactagenda` for in-memory demo
  - Swagger UI tested and working for all CRUD endpoints
  - FluentValidation integrated and enforced in controller actions

- **Frontend Infrastructure Setup**
  - Vue 3 frontend project initialized (contact-agenda-frontend)
  - PrimeVue, axios, vue-router, pinia configured in frontend
  - Navigation between pages using vue-router

- **Frontend CRUD Implementation**
  - Basic Vue view for adding/listing contacts created
  - Frontend CRUD (add, edit, view) implemented and connected to backend
  - Currently using JSON file persistence as temporary database for demonstration

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
  - Fixed API endpoints to use correct JSON persistence routes (/api/contacts)

- **Pinia state management implemented**
  - Centralized contact state management across all components
  - Intelligent caching to prevent unnecessary API calls
  - Optimistic updates for immediate UI feedback
  - Real-time search functionality with computed properties
  - Duplicate email detection before contact creation
  - Automatic error handling and recovery mechanisms

- **Favorite contacts functionality implemented**
  - Added IsFavorite field to Contact model and database schema
  - Heart button (❤️/🤍) for each contact card to toggle favorite status
  - Favorites automatically displayed at top of contact list
  - Smooth animations and visual feedback for favorite interactions
  - Favorite status preserved in both edit forms and new contact creation
  - Backend API endpoint `/api/contacts/{id}/favorite` for toggle operations

## 🚧 Next Steps
1. **Add unit/integration tests for backend**
   - Create a test project and add tests for Service, Repository (including JSON persistence), and Dapper endpoints.
2. ~~**Refactor frontend to use component-based structure**~~ ✅ **COMPLETED**
   - ~~Modularize UI and views for maintainability.~~
3. ~~**Use axios for all API calls in frontend**~~ ✅ **COMPLETED**
   - ~~Replace fetch with axios in Vue components for consistency and better error handling.~~
4. ~~**Implement Pinia for state management (optional)**~~ ✅ **COMPLETED**
   - ~~Use Pinia to manage contacts state in frontend.~~
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
- pinia for state management (optional) ✅ (actively used for centralized contact state)
- Component-based frontend structure ✅
- Frontend test coverage ❌
- Bonus: CQRS, authentication/security, RabbitMQ, Dockerfile ❌

## 🔧 Technical Notes
### API Endpoints
- **`/api/contacts`**: Main CRUD endpoints using JSON file persistence (`contacts.json`) - temporary database substitute
  - `GET /api/contacts` - Retrieve all contacts
  - `GET /api/contacts/{id}` - Retrieve specific contact
  - `POST /api/contacts` - Create new contact
  - `PUT /api/contacts/{id}` - Update existing contact
  - `DELETE /api/contacts/{id}` - Delete contact
  - `PATCH /api/contacts/{id}/favorite` - Toggle favorite status
- **`/api/contactagenda`**: Demo endpoints using in-memory storage
- **`/api/dappercontacts`**: Sample Dapper integration endpoint

### Database Strategy
- **Current**: JSON file (`contacts.json`) serves as temporary database for development/demonstration
- **Production Ready**: EF Core with SQLite database configured and available
- **Repository Pattern**: Allows easy switching between storage implementations
- **Schema**: Contact model includes Id, Name, Email, Phone, and IsFavorite fields

### Frontend Architecture
- **Component Structure**: Organized into `layout/` and `ui/` folders
- **State Management**: Pinia store with centralized contact data, optimistic updates, and intelligent caching
- **API Layer**: Axios-based service layer with centralized error handling
- **Responsive Design**: CSS Grid with automatic column adjustment based on screen width
- **Validation**: Client-side regex validation with server-side FluentValidation backup
- **Search Functionality**: Real-time contact filtering across name, email, and phone fields
- **Favorite System**: Interactive heart buttons with visual feedback and automatic list sorting

## 🔄 How to Switch from JSON to SQLite Database

Currently, the application uses a JSON file (`contacts.json`) as a temporary database substitute. To switch to the production-ready SQLite database:

### **Step 1: Update Dependency Injection in Program.cs**
Navigate to `Program.cs` and replace the JSON repository registration:

**Current (JSON):**
```csharp
// Register JsonContactRepository for DI (all contact CRUD will use contacts.json)
builder.Services.AddSingleton<IContactRepository>(provider =>
    new JsonContactRepository("contacts.json")
);
```

**Change to (SQLite):**
```csharp
// Register ContactRepository for DI (all contact CRUD will use SQLite database)
builder.Services.AddScoped<IContactRepository, ContactRepository>();
```

### **Step 2: Run Database Migrations**
Execute the following commands in the project root directory:
```bash
dotnet ef database update
```

### **Step 3: Verify Database Creation**
- The SQLite database file `ContactAgenda.db` will be created in the project root
- You can inspect it using SQLite browser tools or Visual Studio

### **Step 4: Optional - Seed Initial Data**
If you want to transfer existing JSON data to SQLite:
1. Copy contacts from `contacts.json`
2. Use the `/api/contacts` POST endpoint to add them to the database
3. Or create a data seeding method in `Program.cs`

### **Benefits of SQLite vs JSON:**
- **Performance**: Better query performance and indexing
- **Concurrency**: Better handling of simultaneous requests
- **Relationships**: Support for foreign keys and complex queries
- **Transactions**: ACID compliance for data integrity
- **Scalability**: Can handle larger datasets efficiently

### **Switching Back to JSON:**
Simply reverse Step 1 to return to JSON file persistence for development/testing purposes.

## ⭐ Favorite Contacts Feature

The application includes a comprehensive favorite contacts system that allows users to mark important contacts and access them quickly.

### **How It Works:**
- **Heart Button**: Each contact card displays a heart button (🤍 for non-favorites, ❤️ for favorites)
- **Toggle Functionality**: Click the heart to instantly toggle favorite status with visual feedback
- **Smart Sorting**: Favorite contacts automatically appear at the top of the list
- **Persistent State**: Favorite status is saved and maintained across sessions
- **Form Integration**: New contacts can be marked as favorites during creation
- **Edit Support**: Favorite status can be changed when editing existing contacts

### **Technical Implementation:**
- **Backend**: New `IsFavorite` boolean field added to Contact model and database
- **API Endpoint**: `PATCH /api/contacts/{id}/favorite` for toggling favorite status
- **Frontend**: Pinia store handles optimistic updates with automatic error recovery
- **UI/UX**: Smooth animations and immediate visual feedback for user interactions
- **Data Migration**: Existing contacts automatically receive `IsFavorite: false` as default

### **Database Schema Update:**
```sql
-- Migration: AddIsFavoriteToContact
ALTER TABLE Contacts ADD COLUMN IsFavorite BOOLEAN NOT NULL DEFAULT 0;
```

### **User Experience:**
1. **Visual Distinction**: Favorite contacts show filled heart (❤️) and appear at top of list
2. **Quick Access**: Most important contacts are always visible first
3. **Intuitive Interface**: Familiar heart icon pattern for favoriting
4. **Instant Feedback**: Optimistic updates make the interface feel responsive
5. **Error Handling**: If toggle fails, UI automatically reverts with error message
