# Docker cleanup script for Contact Agenda API (PowerShell)

# Navigate to root directory
$rootPath = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
Set-Location $rootPath

Write-Host "🧹 Cleaning up Contact Agenda API Docker resources..." -ForegroundColor Blue

try {
    # Stop all containers
    Write-Host "🛑 Stopping running containers..." -ForegroundColor Yellow
    docker-compose down

    # Remove containers
    Write-Host "🗑️ Removing containers..." -ForegroundColor Yellow
    $containers = docker ps -a --filter "name=contact-agenda" --format "{{.ID}}"
    if ($containers) {
        docker rm $containers
        Write-Host "   Removed containers: $($containers -join ', ')" -ForegroundColor Gray
    } else {
        Write-Host "   No containers to remove" -ForegroundColor Gray
    }

    # Remove images (optional - ask user)
    $removeImages = Read-Host "🗑️ Remove Docker images? (y/N)"
    if ($removeImages -eq 'y' -or $removeImages -eq 'Y') {
        Write-Host "🗑️ Removing images..." -ForegroundColor Yellow
        $images = docker images --filter "reference=contact-agenda*" --format "{{.ID}}"
        if ($images) {
            docker rmi $images
            Write-Host "   Removed images: $($images -join ', ')" -ForegroundColor Gray
        } else {
            Write-Host "   No images to remove" -ForegroundColor Gray
        }
    }

    # Clean up dangling images and volumes
    Write-Host "🧽 Cleaning up dangling resources..." -ForegroundColor Yellow
    docker system prune -f

    # Show disk space saved
    Write-Host ""
    Write-Host "✅ Cleanup completed!" -ForegroundColor Green
    Write-Host ""
    Write-Host "💾 Docker disk usage:" -ForegroundColor Cyan
    docker system df

    Write-Host ""
    Write-Host "🔄 To rebuild and restart:" -ForegroundColor Green
    Write-Host "   .\scripts\docker-build.ps1" -ForegroundColor White
    Write-Host "   .\scripts\docker-run.ps1" -ForegroundColor White
}
catch {
    Write-Error "❌ Cleanup failed: $_"
    exit 1
}
