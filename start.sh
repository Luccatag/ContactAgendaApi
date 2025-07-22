#!/bin/bash
# Quick start script for Contact Agenda API
# This script runs the full development environment

echo "🚀 Starting Contact Agenda API..."
echo "====================================="

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "❌ Error: Docker is not running. Please start Docker Desktop."
    exit 1
fi

# Start the application
docker-compose up -d

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Application started successfully!"
    echo ""
    echo "🌐 Access your application:"
    echo "   Frontend: http://localhost:3000"
    echo "   API:      http://localhost:8081"
    echo "   Swagger:  http://localhost:8081/swagger"
    echo ""
    echo "📋 Useful commands:"
    echo "   Check status: docker-compose ps"
    echo "   View logs:    docker-compose logs -f"
    echo "   Stop app:     docker-compose down"
    echo ""
    echo "📚 For more options, see: ./docker/scripts/"
else
    echo "❌ Failed to start application. Check the logs for details."
    exit 1
fi
