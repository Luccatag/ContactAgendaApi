# Docker build script for Contact Agenda API (PowerShell)

Write-Host "🐳 Building Contact Agenda API Docker Images..." -ForegroundColor Blue

try {
    # Build backend
    Write-Host "📦 Building backend image..." -ForegroundColor Yellow
    docker build -t contact-agenda-api:latest .

    # Build frontend
    Write-Host "🎨 Building frontend image..." -ForegroundColor Yellow
    docker build -t contact-agenda-frontend:latest ./contact-agenda-frontend

    # Build with Docker Compose
    Write-Host "🔧 Building with Docker Compose..." -ForegroundColor Yellow
    docker-compose build

    Write-Host "✅ Docker images built successfully!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Available images:" -ForegroundColor Cyan
    docker images | Where-Object { $_ -match "contact-agenda" }

    Write-Host ""
    Write-Host "🚀 To run the application:" -ForegroundColor Green
    Write-Host "   docker-compose up" -ForegroundColor White
    Write-Host ""
    Write-Host "🧪 To run in development mode:" -ForegroundColor Green
    Write-Host "   docker-compose up --build" -ForegroundColor White
}
catch {
    Write-Error "❌ Build failed: $_"
    exit 1
}
