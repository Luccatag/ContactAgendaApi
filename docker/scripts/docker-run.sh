#!/bin/bash
# Docker run script for Contact Agenda API

set -e

echo "🚀 Starting Contact Agenda API..."

# Check if images exist
if ! docker images | grep -q contact-agenda; then
    echo "⚠️  Images not found. Building first..."
    ./scripts/docker-build.sh
fi

# Navigate to root directory
cd "$(dirname "$0")/../.."

# Start services
echo "🔄 Starting services with Docker Compose..."
docker-compose up -d

# Wait for services to be healthy
echo "⏳ Waiting for services to be ready..."
sleep 10

# Check service status
echo "📊 Service Status:"
docker-compose ps

# Show logs
echo ""
echo "📝 Recent logs:"
docker-compose logs --tail=20

echo ""
echo "✅ Application is running!"
echo "🌐 Frontend: http://localhost:3000"
echo "🔧 Backend API: http://localhost:8081"
echo "📚 Swagger UI: http://localhost:8081/swagger"
echo ""
echo "🛑 To stop: docker-compose down"
echo "📋 To view logs: docker-compose logs -f"
