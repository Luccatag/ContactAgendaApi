# 🐳 Docker Run Guide - Contact Agenda API### 2. `docker-compose-simple.yml` (Alternative)
- **Purpose**: Lightweight production setup  
- **Features**: 
  - API backend with SQLite database
  - Vue.js frontend with nginx proxy
  - Lightweight configuration
- **Use Case**: When you need minimal resource usage

```bash
docker-compose -f docker-compose-simple.yml up -d
```ide explains how to run the Contact Agenda application using Docker in different configurations.

## 📋 Prerequisites

- Docker Desktop installed and running
- Git (to clone the repository)
- At least 2GB of free disk space

## 🚀 Quick Start (Recommended)

### Fullstack Configuration
```bash
# Clone the repository
git clone https://github.com/Luccatag/ContactAgendaApi.git
cd ContactAgendaApi

# Quick start (uses default fullstack configuration)
docker-compose up -d

# OR use the convenience scripts
./start.sh              # Linux/macOS
.\start.ps1             # Windows PowerShell

# Access the application
# Frontend: http://localhost:3000
# API: http://localhost:8081
```

### Using Organized Scripts
For more advanced usage, check the organized scripts in `docker/scripts/`:

```bash
# Linux/macOS
./docker/scripts/docker-run.sh start
./docker/scripts/docker-run.sh logs
./docker/scripts/docker-run.sh clean

# Windows PowerShell
.\docker\scripts\docker-run.ps1 start
.\docker\scripts\docker-run.ps1 logs
.\docker\scripts\docker-run.ps1 clean
```

## 📁 Available Docker Compose Files

### 1. `docker-compose-fullstack.yml` ⭐ **Recommended**
- **Purpose**: Complete development environment
- **Features**:
  - Full backend API with SQLite database
  - Vue.js frontend with nginx proxy
  - Health checks enabled
  - Volume for data persistence
  - Optimized for both development and production
- **Use Case**: Primary deployment method

```bash
docker-compose up -d
```

### 2. `docker/compose/docker-compose-simple.yml` (Alternative)
- **Purpose**: Minimal setup (alternative)
- **Features**: 
  - API backend with SQLite database
  - Vue.js frontend with nginx proxy
  - Lightweight configuration
- **Use Case**: When you need minimal resource usage

```bash
docker-compose -f docker/compose/docker-compose-simple.yml up -d
```

### 3. `docker/compose/docker-compose-database.yml`
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
# Start all services in background (using default configuration)
docker-compose up -d

# Start with logs visible
docker-compose up

# Using specific configuration
docker-compose -f docker/compose/docker-compose-fullstack.yml up -d

# Start specific service
docker-compose up -d api
```

### Stop Services
```bash
# Stop all services
docker-compose down

# Stop and remove volumes
docker-compose down -v

# Stop specific service
docker-compose stop api
```

### View Logs
```bash
# View all logs
docker-compose logs

# View specific service logs
docker-compose logs api
docker-compose logs frontend

# Follow logs in real-time
docker-compose logs -f
```

### Check Status
```bash
# View running containers
docker-compose ps

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
docker-compose down
rm contacts.db
docker-compose up -d
```

### Rebuild Containers
```bash
# Rebuild and restart
docker-compose down
docker-compose build --no-cache
docker-compose up -d
```

## 🧹 Cleanup Commands

### Remove Everything
```bash
# Stop and remove containers, networks, volumes
docker-compose down -v

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
