# Docker Configuration

This folder contains all Docker-related files for the Contact Agenda API project.

## 📁 Folder Structure

```
docker/
├── compose/                    # Docker Compose configurations
│   ├── docker-compose-fullstack.yml    # Complete development environment (recommended)
│   ├── docker-compose-simple.yml       # Lightweight production setup
│   ├── docker-compose-database.yml     # Enhanced database support
│   ├── docker-compose-mysql.yml        # MySQL database option
│   └── docker-compose.override.yml     # Override configuration
├── dockerfiles/               # Specialized Dockerfiles
│   ├── Dockerfile.api-only    # API container only
│   ├── Dockerfile.development # Development optimized
│   └── Dockerfile.simple      # Simple production build
└── scripts/                   # Docker utility scripts
    ├── docker-run.sh          # Linux/macOS run script
    ├── docker-run.ps1         # Windows PowerShell script
    ├── docker-run.bat         # Windows batch script
    ├── docker-run-simple.ps1  # Simplified PowerShell script
    ├── docker-build.sh        # Build scripts
    ├── docker-build.ps1       # Build scripts (PowerShell)
    ├── docker-cleanup.sh      # Cleanup scripts
    └── docker-cleanup.ps1     # Cleanup scripts (PowerShell)
```

## 🚀 Quick Start

For the simplest start, use the root-level scripts:

### Linux/macOS
```bash
./start.sh
```

### Windows
```powershell
.\start.ps1
```

Or use the default docker-compose:
```bash
docker-compose up -d
```

## 📋 Docker Compose Files

### Primary Configurations

- **`docker-compose-fullstack.yml`** ⭐ **Recommended**
  - Complete development environment
  - Backend API + Frontend
  - Health checks enabled
  - Volume persistence
  - Optimized for development and production

- **`docker-compose-simple.yml`**
  - Lightweight alternative
  - Minimal resource usage
  - Production-ready

### Additional Configurations

- **`docker-compose-database.yml`**
  - Enhanced database features
  - Database management tools
  - Advanced persistence options

- **`docker-compose-mysql.yml`**
  - MySQL database backend
  - phpMyAdmin included
  - Enterprise-grade setup

## 🛠️ Specialized Dockerfiles

- **`Dockerfile.api-only`**: API container without frontend
- **`Dockerfile.development`**: Development optimized build
- **`Dockerfile.simple`**: Minimal production build

## 📚 Scripts

All scripts provide help when run without arguments. Examples:

```bash
# Linux/macOS
./docker/scripts/docker-run.sh help

# Windows
.\docker\scripts\docker-run.ps1 help
```

## 🔧 Usage Examples

### Using specific configurations
```bash
# Fullstack (recommended)
docker-compose -f docker/compose/docker-compose-fullstack.yml up -d

# Simple setup
docker-compose -f docker/compose/docker-compose-simple.yml up -d

# MySQL setup
docker-compose -f docker/compose/docker-compose-mysql.yml up -d
```

### Using scripts
```bash
# Start application
./docker/scripts/docker-run.sh start

# View logs
./docker/scripts/docker-run.sh logs

# Clean up
./docker/scripts/docker-run.sh clean
```

## 📞 Support

For issues with Docker setup:
1. Check `DOCKER-RUN-GUIDE.md` in the root directory
2. Verify Docker Desktop is running
3. Try rebuilding with `--no-cache`
4. Check port availability (3000, 8081)
