#!/bin/bash
# 🐳 Quick Docker Run Script for Contact Agenda API
# This script provides quick commands to run the application

echo "🚀 Contact Agenda API - Docker Quick Start"
echo "=========================================="

# Function to show usage
show_usage() {
    echo ""
    echo "Usage: ./docker-run.sh [COMMAND]"
    echo ""
    echo "Commands:"
    echo "  start     - Start the application (recommended)"
    echo "  stop      - Stop the application"
    echo "  restart   - Restart the application"
    echo "  logs      - View application logs"
    echo "  status    - Check application status"
    echo "  clean     - Clean up Docker resources"
    echo "  rebuild   - Rebuild and restart application"
    echo "  help      - Show this help message"
    echo ""
    echo "Examples:"
    echo "  ./docker-run.sh start    # Start the application"
    echo "  ./docker-run.sh logs     # View logs"
    echo "  ./docker-run.sh clean    # Clean up Docker"
    echo ""
}

# Function to start the application
start_app() {
    echo "🟢 Starting Contact Agenda Application..."
    echo "Using docker-compose-simple.yml (recommended configuration)"
    
    # Check if Docker is running
    if ! docker info > /dev/null 2>&1; then
        echo "❌ Error: Docker is not running. Please start Docker Desktop."
        exit 1
    fi
    
    # Start the application
    docker-compose -f docker-compose-simple.yml up -d
    
    if [ $? -eq 0 ]; then
        echo ""
        echo "✅ Application started successfully!"
        echo ""
        echo "🌐 Access your application:"
        echo "   Frontend: http://localhost:3000"
        echo "   API:      http://localhost:8081"
        echo "   Swagger:  http://localhost:8081/swagger"
        echo ""
        echo "📊 Check status: ./docker-run.sh status"
        echo "📝 View logs:    ./docker-run.sh logs"
    else
        echo "❌ Failed to start application. Check the logs for details."
    fi
}

# Function to stop the application
stop_app() {
    echo "🔴 Stopping Contact Agenda Application..."
    docker-compose -f docker-compose-simple.yml down
    
    if [ $? -eq 0 ]; then
        echo "✅ Application stopped successfully!"
    else
        echo "❌ Failed to stop application."
    fi
}

# Function to restart the application
restart_app() {
    echo "🔄 Restarting Contact Agenda Application..."
    stop_app
    echo ""
    start_app
}

# Function to view logs
view_logs() {
    echo "📝 Viewing application logs..."
    echo "Press Ctrl+C to exit log view"
    echo ""
    docker-compose -f docker-compose-simple.yml logs -f
}

# Function to check status
check_status() {
    echo "📊 Application Status:"
    echo "====================="
    
    # Check if containers are running
    if docker-compose -f docker-compose-simple.yml ps | grep -q "Up"; then
        echo "🟢 Application is RUNNING"
        echo ""
        docker-compose -f docker-compose-simple.yml ps
        echo ""
        echo "🌐 Application URLs:"
        echo "   Frontend: http://localhost:3000"
        echo "   API:      http://localhost:8081"
    else
        echo "🔴 Application is STOPPED"
        echo ""
        echo "Start the application with: ./docker-run.sh start"
    fi
}

# Function to clean Docker resources
clean_docker() {
    echo "🧹 Cleaning up Docker resources..."
    
    # Stop the application first
    docker-compose -f docker-compose-simple.yml down
    
    # Remove unused containers, networks, images, and build cache
    docker system prune -f
    
    # Remove build cache
    docker builder prune -f
    
    echo "✅ Docker cleanup completed!"
    echo "💾 Space freed up. You can check with: docker system df"
}

# Function to rebuild application
rebuild_app() {
    echo "🔨 Rebuilding Contact Agenda Application..."
    
    # Stop application
    docker-compose -f docker-compose-simple.yml down
    
    # Rebuild images
    docker-compose -f docker-compose-simple.yml build --no-cache
    
    # Start application
    docker-compose -f docker-compose-simple.yml up -d
    
    if [ $? -eq 0 ]; then
        echo "✅ Application rebuilt and started successfully!"
        echo ""
        echo "🌐 Access your application:"
        echo "   Frontend: http://localhost:3000"
        echo "   API:      http://localhost:8081"
    else
        echo "❌ Failed to rebuild application."
    fi
}

# Main script logic
case "${1:-help}" in
    start)
        start_app
        ;;
    stop)
        stop_app
        ;;
    restart)
        restart_app
        ;;
    logs)
        view_logs
        ;;
    status)
        check_status
        ;;
    clean)
        clean_docker
        ;;
    rebuild)
        rebuild_app
        ;;
    help|--help|-h)
        show_usage
        ;;
    *)
        echo "❌ Unknown command: $1"
        show_usage
        exit 1
        ;;
esac
