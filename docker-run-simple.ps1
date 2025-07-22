# Quick Docker Run Script for Contact Agenda API (PowerShell)
# This script provides quick commands to run the application on Windows

param(
    [Parameter(Position=0)]
    [string]$Command = "help"
)

Write-Host "Contact Agenda API - Docker Quick Start" -ForegroundColor Cyan
Write-Host "=======================================" -ForegroundColor Cyan

function Show-Usage {
    Write-Host ""
    Write-Host "Usage: .\docker-run.ps1 [COMMAND]" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Commands:" -ForegroundColor White
    Write-Host "  start     - Start the application (recommended)" -ForegroundColor Green
    Write-Host "  stop      - Stop the application" -ForegroundColor Red
    Write-Host "  restart   - Restart the application" -ForegroundColor Yellow
    Write-Host "  logs      - View application logs" -ForegroundColor Blue
    Write-Host "  status    - Check application status" -ForegroundColor Cyan
    Write-Host "  clean     - Clean up Docker resources" -ForegroundColor Magenta
    Write-Host "  rebuild   - Rebuild and restart application" -ForegroundColor DarkYellow
    Write-Host "  help      - Show this help message" -ForegroundColor White
    Write-Host ""
    Write-Host "Examples:" -ForegroundColor Yellow
    Write-Host "  .\docker-run.ps1 start    # Start the application"
    Write-Host "  .\docker-run.ps1 logs     # View logs"
    Write-Host "  .\docker-run.ps1 clean    # Clean up Docker"
    Write-Host ""
}

function Start-App {
    Write-Host "Starting Contact Agenda Application..." -ForegroundColor Green
    Write-Host "Using docker-compose-simple.yml (recommended configuration)" -ForegroundColor Yellow
    
    # Check if Docker is running
    try {
        docker info | Out-Null
    }
    catch {
        Write-Host "Error: Docker is not running. Please start Docker Desktop." -ForegroundColor Red
        exit 1
    }
    
    # Start the application
    docker-compose -f docker-compose-simple.yml up -d
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "Application started successfully!" -ForegroundColor Green
        Write-Host ""
        Write-Host "Access your application:" -ForegroundColor Cyan
        Write-Host "   Frontend: http://localhost:3000" -ForegroundColor White
        Write-Host "   API:      http://localhost:8081" -ForegroundColor White
        Write-Host "   Swagger:  http://localhost:8081/swagger" -ForegroundColor White
        Write-Host ""
        Write-Host "Check status: .\docker-run.ps1 status" -ForegroundColor Yellow
        Write-Host "View logs:    .\docker-run.ps1 logs" -ForegroundColor Yellow
    }
    else {
        Write-Host "Failed to start application. Check the logs for details." -ForegroundColor Red
    }
}

function Stop-App {
    Write-Host "Stopping Contact Agenda Application..." -ForegroundColor Red
    docker-compose -f docker-compose-simple.yml down
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Application stopped successfully!" -ForegroundColor Green
    }
    else {
        Write-Host "Failed to stop application." -ForegroundColor Red
    }
}

function Restart-App {
    Write-Host "Restarting Contact Agenda Application..." -ForegroundColor Yellow
    Stop-App
    Write-Host ""
    Start-App
}

function View-Logs {
    Write-Host "Viewing application logs..." -ForegroundColor Blue
    Write-Host "Press Ctrl+C to exit log view" -ForegroundColor Yellow
    Write-Host ""
    docker-compose -f docker-compose-simple.yml logs -f
}

function Check-Status {
    Write-Host "Application Status:" -ForegroundColor Cyan
    Write-Host "==================" -ForegroundColor Cyan
    
    # Check if containers are running
    $runningContainers = docker-compose -f docker-compose-simple.yml ps | Select-String "Up"
    
    if ($runningContainers) {
        Write-Host "Application is RUNNING" -ForegroundColor Green
        Write-Host ""
        docker-compose -f docker-compose-simple.yml ps
        Write-Host ""
        Write-Host "Application URLs:" -ForegroundColor Cyan
        Write-Host "   Frontend: http://localhost:3000" -ForegroundColor White
        Write-Host "   API:      http://localhost:8081" -ForegroundColor White
    }
    else {
        Write-Host "Application is STOPPED" -ForegroundColor Red
        Write-Host ""
        Write-Host "Start the application with: .\docker-run.ps1 start" -ForegroundColor Yellow
    }
}

function Clean-Docker {
    Write-Host "Cleaning up Docker resources..." -ForegroundColor Magenta
    
    # Stop the application first
    docker-compose -f docker-compose-simple.yml down
    
    # Remove unused containers, networks, images, and build cache
    docker system prune -f
    
    # Remove build cache
    docker builder prune -f
    
    Write-Host "Docker cleanup completed!" -ForegroundColor Green
    Write-Host "Space freed up. You can check with: docker system df" -ForegroundColor Yellow
}

function Rebuild-App {
    Write-Host "Rebuilding Contact Agenda Application..." -ForegroundColor DarkYellow
    
    # Stop application
    docker-compose -f docker-compose-simple.yml down
    
    # Rebuild images
    docker-compose -f docker-compose-simple.yml build --no-cache
    
    # Start application
    docker-compose -f docker-compose-simple.yml up -d
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Application rebuilt and started successfully!" -ForegroundColor Green
        Write-Host ""
        Write-Host "Access your application:" -ForegroundColor Cyan
        Write-Host "   Frontend: http://localhost:3000" -ForegroundColor White
        Write-Host "   API:      http://localhost:8081" -ForegroundColor White
    }
    else {
        Write-Host "Failed to rebuild application." -ForegroundColor Red
    }
}

# Main script logic
switch ($Command.ToLower()) {
    "start" {
        Start-App
    }
    "stop" {
        Stop-App
    }
    "restart" {
        Restart-App
    }
    "logs" {
        View-Logs
    }
    "status" {
        Check-Status
    }
    "clean" {
        Clean-Docker
    }
    "rebuild" {
        Rebuild-App
    }
    "help" {
        Show-Usage
    }
    default {
        Write-Host "Unknown command: $Command" -ForegroundColor Red
        Show-Usage
        exit 1
    }
}
