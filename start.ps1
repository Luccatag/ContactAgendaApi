# Quick start script for Contact Agenda API (PowerShell)
# This script runs the full development environment

Write-Host "Contact Agenda API - Quick Start" -ForegroundColor Cyan
Write-Host "=================================" -ForegroundColor Cyan

# Check if Docker is running
try {
    docker info | Out-Null
}
catch {
    Write-Host "Error: Docker is not running. Please start Docker Desktop." -ForegroundColor Red
    exit 1
}

# Start the application
Write-Host "Starting application..." -ForegroundColor Yellow
docker-compose up -d

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "Application started successfully!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Access your application:" -ForegroundColor Cyan
    Write-Host "   Frontend: http://localhost:3000" -ForegroundColor White
    Write-Host "   API:      http://localhost:8081" -ForegroundColor White
    Write-Host "   Swagger:  http://localhost:8081/swagger" -ForegroundColor White
    Write-Host ""
    Write-Host "Useful commands:" -ForegroundColor Yellow
    Write-Host "   Check status: docker-compose ps" -ForegroundColor White
    Write-Host "   View logs:    docker-compose logs -f" -ForegroundColor White
    Write-Host "   Stop app:     docker-compose down" -ForegroundColor White
    Write-Host ""
    Write-Host "For more options, see: .\docker\scripts\" -ForegroundColor Cyan
}
else {
    Write-Host "Failed to start application. Check the logs for details." -ForegroundColor Red
    exit 1
}
