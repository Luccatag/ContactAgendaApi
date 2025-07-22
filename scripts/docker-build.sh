#!/bin/bash
# Docker build script for Contact Agenda API

set -e

echo "🐳 Building Contact Agenda API Docker Images..."

# Build backend
echo "📦 Building backend image..."
docker build -t contact-agenda-api:latest .

# Build frontend
echo "🎨 Building frontend image..."
docker build -t contact-agenda-frontend:latest ./contact-agenda-frontend

# Build with Docker Compose
echo "🔧 Building with Docker Compose..."
docker-compose build

echo "✅ Docker images built successfully!"
echo ""
echo "Available images:"
docker images | grep contact-agenda

echo ""
echo "🚀 To run the application:"
echo "   docker-compose up"
echo ""
echo "🧪 To run in development mode:"
echo "   docker-compose up --build"
