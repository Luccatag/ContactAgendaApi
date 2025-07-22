# Docker Desktop Quick Start Guide

## 🐳 Running Contact Agenda API in Docker Desktop

### Prerequisites
- Docker Desktop installed and running
- Git Bash or PowerShell (Windows)

### Quick Start - Single Backend Container

#### 1. Build the API-Only Container
```bash
# In the ContactAgendaApi directory
docker build -f Dockerfile.api-only -t contact-agenda-api:latest .
```

#### 2. Run the Container
```bash
docker run -d -p 8080:5000 --name contact-agenda-api contact-agenda-api:latest
```

#### 3. Test the API
Open your browser or use curl:
- Weather endpoint: http://localhost:8080/weatherforecast
- API base: http://localhost:8080/api/Contacts

### 🎛️ Using Docker Desktop GUI

#### Running Containers:
1. Open Docker Desktop
2. Go to "Images" tab
3. Find `contact-agenda-api:latest`
4. Click "Run" button
5. Configure:
   - Container name: `contact-agenda-api`
   - Port: `8080:5000`
   - Click "Run"

#### Managing Containers:
1. Go to "Containers" tab
2. See your running `contact-agenda-api`
3. Use buttons to:
   - ▶️ Start/Stop
   - 🗑️ Delete
   - 📊 View logs
   - 🔗 Open in browser (click port 8080)

### 🔧 Available Commands

#### Container Management:
```bash
# View running containers
docker ps

# Check container logs
docker logs contact-agenda-api

# Stop container
docker stop contact-agenda-api

# Remove container
docker rm contact-agenda-api

# Remove image
docker rmi contact-agenda-api:latest
```

#### PowerShell Testing:
```powershell
# Test API endpoints
Invoke-WebRequest -Uri http://localhost:8080/weatherforecast
Invoke-WebRequest -Uri http://localhost:8080/api/Contacts
```

### 🌐 API Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/weatherforecast` | GET | Test endpoint |
| `/api/Contacts` | GET | Get all contacts |
| `/api/Contacts/{id}` | GET | Get contact by ID |
| `/api/Contacts` | POST | Create contact |
| `/api/Contacts/{id}` | PUT | Update contact |
| `/api/Contacts/{id}` | DELETE | Delete contact |

### 📝 Sample Contact JSON:
```json
{
  "name": "John Doe",
  "email": "john@example.com",
  "phone": "555-0123",
  "isFavorite": false
}
```

### ⚠️ Known Issues & Solutions

#### Issue: 500 Internal Server Error on /api/Contacts
**Cause:** Database not initialized
**Solution:** The API uses SQLite and should auto-create the database. If issues persist, check container logs.

#### Issue: CQRS endpoints not working
**Cause:** MediatR registration commented out in production build
**Solution:** Use regular `/api/Contacts` endpoints for now.

#### Issue: Swagger not accessible
**Cause:** Swagger disabled in Production mode
**Solution:** Use the endpoints directly or enable development mode.

### 🔍 Troubleshooting

1. **Check if container is running:**
   ```bash
   docker ps
   ```

2. **View container logs:**
   ```bash
   docker logs contact-agenda-api
   ```

3. **Access container shell:**
   ```bash
   docker exec -it contact-agenda-api /bin/bash
   ```

4. **Check port mapping:**
   - Ensure port 8080 is not used by other applications
   - Try different port: `-p 9080:5000`

### 🎯 Next Steps

After successfully running the basic container:
1. Test all API endpoints
2. Try building the full multi-container setup with docker-compose
3. Add frontend container for complete application stack

### 📞 Support

If you encounter issues:
1. Check Docker Desktop is running
2. Ensure port 8080 is available
3. Review container logs for errors
4. Try rebuilding the image if changes were made
