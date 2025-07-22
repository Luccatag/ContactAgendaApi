# Docker run script for Contact Agenda API (PowerShell)

Write-Host "🚀 Starting Contact Agenda API..." -ForegroundColor Blue

try {
    # Check if Docker is running
    $dockerStatus = docker version 2>$null
    if ($LASTEXITCODE -ne 0) {
        Write-Error "❌ Docker is not running. Please start Docker Desktop."
        exit 1
    }

    # Run the application
    Write-Host "🔄 Starting services with Docker Compose..." -ForegroundColor Yellow
    docker-compose up -d

    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "✅ Contact Agenda API is now running!" -ForegroundColor Green
        Write-Host ""
        Write-Host "🌐 Available endpoints:" -ForegroundColor Cyan
        Write-Host "   Frontend:     http://localhost:8080" -ForegroundColor White
        Write-Host "   Backend API:  http://localhost:5000" -ForegroundColor White
        Write-Host "   API Docs:     http://localhost:5000/swagger" -ForegroundColor White
        Write-Host ""
        Write-Host "📋 To view logs:" -ForegroundColor Green
        Write-Host "   docker-compose logs -f" -ForegroundColor White
        Write-Host ""
        Write-Host "🛑 To stop the application:" -ForegroundColor Green
        Write-Host "   docker-compose down" -ForegroundColor White
        Write-Host ""
        Write-Host "📊 To view running containers:" -ForegroundColor Green
        Write-Host "   docker ps" -ForegroundColor White
    } else {
        Write-Error "❌ Failed to start the application"
        exit 1
    }
}
catch {
    Write-Error "❌ Error starting application: $_"
    exit 1
}
