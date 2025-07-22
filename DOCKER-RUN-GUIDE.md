# 🐳 Docker Run Guide - Contact Agenda API

This guide explains how to run the Contact Agenda application using Docker in different configurations.

## 📋 Prerequisites

- Docker Desktop installed and running
- Git (to clone the repository)
- At least 2GB of free disk space

## 🚀 Quick Start (Recommended)

### Option 1: Simple Configuration (Fastest)
```bash
# Clone the repository
git clone https://github.com/Luccatag/ContactAgendaApi.git
cd ContactAgendaApi

# Run the application
docker-compose -f docker-compose-simple.yml up -d

# Access the application
# Frontend: http://localhost:3000
# API: http://localhost:8081
```

### Option 2: Full Stack Configuration
```bash
# Run with full stack setup
docker-compose -f docker-compose-fullstack.yml up -d

# Access the application
# Frontend: http://localhost:3000
# API: http://localhost:8081
```

## 📁 Available Docker Compose Files

### 1. `docker-compose-simple.yml` ⭐ **Recommended**
- **Purpose**: Production-ready minimal setup
- **Features**: 
  - API backend with SQLite database
  - Vue.js frontend with nginx proxy
  - Optimized for performance
- **Use Case**: Daily development and production deployment

```bash
docker-compose -f docker-compose-simple.yml up -d
```

### 2. `docker-compose-fullstack.yml`
- **Purpose**: Complete development environment
- **Features**:
  - Full backend API
  - Frontend with all development tools
  - Health checks enabled
  - Volume for data persistence
- **Use Case**: Full-featured development

```bash
docker-compose -f docker-compose-fullstack.yml up -d
```

### 3. `docker-compose-database.yml`
- **Purpose**: Enhanced database support
- **Features**:
  - Multiple database options
  - Database management tools
  - Enhanced persistence
- **Use Case**: Database-focused development

```bash
docker-compose -f docker-compose-database.yml up -d
```

### 4. `docker-compose-mysql.yml`
- **Purpose**: MySQL database backend
- **Features**:
  - MySQL database instead of SQLite
  - phpMyAdmin for database management
  - Enterprise-grade database setup
- **Use Case**: Production with MySQL

```bash
docker-compose -f docker-compose-mysql.yml up -d
```

## 🔧 Individual Container Commands

### Build and Run API Only
```bash
# Build the API image
docker build -f Dockerfile.api-only -t contact-agenda-api:latest .

# Run API container
docker run -d \
  --name contact-agenda-api \
  -p 8081:5000 \
  -v $(pwd)/contacts.db:/app/contacts.db \
  -e ASPNETCORE_ENVIRONMENT=Development \
  contact-agenda-api:latest
```

### Build and Run Frontend Only
```bash
# Build the frontend image
docker build -t contact-agenda-frontend:latest ./contact-agenda-frontend

# Run frontend container
docker run -d \
  --name contact-agenda-frontend \
  -p 3000:8080 \
  contact-agenda-frontend:latest
```

## 📊 Service Details

### Backend API (ASP.NET Core)
- **Port**: 8081 (external) → 5000 (internal)
- **Database**: SQLite (file-based)
- **Health Check**: `http://localhost:8081/health`
- **API Documentation**: `http://localhost:8081/swagger`

### Frontend (Vue.js + Nginx)
- **Port**: 3000 (external) → 8080 (internal)
- **Technology**: Vue.js 3 with Vite
- **Proxy**: API calls automatically routed to backend
- **Health Check**: `http://localhost:3000/health`

## 🗂️ Data Persistence

### SQLite Database
- **File**: `./contacts.db`
- **Location**: Mapped to `/app/contacts.db` in container
- **Persistence**: Data survives container restarts

### Volume Mapping
```bash
# The database file is automatically created and persisted
volumes:
  - ./contacts.db:/app/contacts.db
```

## 🛠️ Common Commands

### Start Services
```bash
# Start all services in background
docker-compose -f docker-compose-simple.yml up -d

# Start with logs visible
docker-compose -f docker-compose-simple.yml up

# Start specific service
docker-compose -f docker-compose-simple.yml up -d api
```

### Stop Services
```bash
# Stop all services
docker-compose -f docker-compose-simple.yml down

# Stop and remove volumes
docker-compose -f docker-compose-simple.yml down -v

# Stop specific service
docker-compose -f docker-compose-simple.yml stop api
```

### View Logs
```bash
# View all logs
docker-compose -f docker-compose-simple.yml logs

# View specific service logs
docker-compose -f docker-compose-simple.yml logs api
docker-compose -f docker-compose-simple.yml logs frontend

# Follow logs in real-time
docker-compose -f docker-compose-simple.yml logs -f
```

### Check Status
```bash
# View running containers
docker-compose -f docker-compose-simple.yml ps

# Check health status
docker ps
```

## 🔍 Troubleshooting

### Port Already in Use
```bash
# Check what's using the port
netstat -ano | findstr :3000
netstat -ano | findstr :8081

# Kill process using the port (Windows)
taskkill /PID <PID_NUMBER> /F
```

### Container Won't Start
```bash
# Check container logs
docker logs contact-agenda-api
docker logs contact-agenda-frontend

# Check container status
docker ps -a
```

### Database Issues
```bash
# Reset database (removes all data)
docker-compose -f docker-compose-simple.yml down
rm contacts.db
docker-compose -f docker-compose-simple.yml up -d
```

### Rebuild Containers
```bash
# Rebuild and restart
docker-compose -f docker-compose-simple.yml down
docker-compose -f docker-compose-simple.yml build --no-cache
docker-compose -f docker-compose-simple.yml up -d
```

## 🧹 Cleanup Commands

### Remove Everything
```bash
# Stop and remove containers, networks, volumes
docker-compose -f docker-compose-simple.yml down -v

# Remove all unused Docker resources
docker system prune -a -f

# Remove specific images
docker rmi contact-agenda-api contact-agenda-frontend
```

### Clean Build Cache
```bash
# Remove build cache to free space
docker builder prune -a -f
```

## 🌐 Environment Variables

### Backend Environment
```bash
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://+:5000
```

### Database Configuration
```bash
# SQLite connection string (default)
ConnectionStrings__DefaultConnection=Data Source=contacts.db

# For MySQL setup
MYSQL_ROOT_PASSWORD=your_password
MYSQL_DATABASE=contactagenda
```

## 📱 Testing the Application

### API Endpoints
```bash
# Get all contacts
curl http://localhost:8081/api/contacts

# Create a new contact
curl -X POST http://localhost:8081/api/contacts \
  -H "Content-Type: application/json" \
  -d '{"name":"John Doe","email":"john@example.com","phone":"1234567890","isFavorite":false}'

# Health check
curl http://localhost:8081/health
```

### Frontend Testing
```bash
# Access the application
open http://localhost:3000

# Test API proxy through frontend
curl http://localhost:3000/api/contacts
```

## 🔐 Security Notes

- Default configuration is for development only
- Change default passwords in production
- Use environment files (.env) for sensitive data
- Consider using Docker secrets for production

## 📞 Support

If you encounter issues:
1. Check the logs: `docker-compose logs`
2. Verify ports are not in use
3. Ensure Docker Desktop is running
4. Try rebuilding containers with `--no-cache`

## 🏷️ Available Tags

- `latest` - Most recent stable build
- `dev` - Development build
- `api-only` - API container only

---

**Happy Dockerizing! 🐳**
