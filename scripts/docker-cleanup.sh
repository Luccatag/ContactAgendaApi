#!/bin/bash
# Docker cleanup script for Contact Agenda API

set -e

echo "🧹 Cleaning up Docker resources for Contact Agenda API..."

# Stop and remove containers
echo "🛑 Stopping containers..."
docker-compose down

# Remove images (optional)
read -p "🗑️  Remove Docker images? (y/N): " remove_images
if [[ $remove_images =~ ^[Yy]$ ]]; then
    echo "🗑️  Removing images..."
    docker rmi contact-agenda-api:latest 2>/dev/null || true
    docker rmi contact-agenda-frontend:latest 2>/dev/null || true
fi

# Remove volumes (optional)
read -p "💾 Remove data volumes? (y/N): " remove_volumes
if [[ $remove_volumes =~ ^[Yy]$ ]]; then
    echo "⚠️  Removing data volumes (this will delete database)..."
    docker-compose down -v
fi

# Clean up Docker system (optional)
read -p "🧽 Run Docker system cleanup? (y/N): " system_cleanup
if [[ $system_cleanup =~ ^[Yy]$ ]]; then
    echo "🧽 Running Docker system cleanup..."
    docker system prune -f
fi

echo "✅ Cleanup completed!"
echo ""
echo "🔄 To rebuild and run:"
echo "   ./scripts/docker-build.sh"
echo "   ./scripts/docker-run.sh"
