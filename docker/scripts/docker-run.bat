@echo off
REM 🐳 Quick Docker Run Script for Contact Agenda API (Windows Batch)
REM This script provides quick commands to run the application on Windows

echo 🚀 Contact Agenda API - Docker Quick Start
echo ===========================================

if "%1"=="" goto help
if "%1"=="help" goto help
if "%1"=="-h" goto help
if "%1"=="--help" goto help
if "%1"=="start" goto start
if "%1"=="stop" goto stop
if "%1"=="restart" goto restart
if "%1"=="logs" goto logs
if "%1"=="status" goto status
if "%1"=="clean" goto clean
if "%1"=="rebuild" goto rebuild

echo ❌ Unknown command: %1
goto help

:help
echo.
echo Usage: docker-run.bat [COMMAND]
echo.
echo Commands:
echo   start     - Start the application (recommended)
echo   stop      - Stop the application
echo   restart   - Restart the application
echo   logs      - View application logs
echo   status    - Check application status
echo   clean     - Clean up Docker resources
echo   rebuild   - Rebuild and restart application
echo   help      - Show this help message
echo.
echo Examples:
echo   docker-run.bat start    # Start the application
echo   docker-run.bat logs     # View logs
echo   docker-run.bat clean    # Clean up Docker
echo.
goto end

:start
echo 🟢 Starting Contact Agenda Application...
echo Using docker-compose-fullstack.yml (recommended configuration)

REM Check if Docker is running
docker info >nul 2>&1
if errorlevel 1 (
    echo ❌ Error: Docker is not running. Please start Docker Desktop.
    goto end
)

REM Start the application
docker-compose -f docker-compose-fullstack.yml up -d

if errorlevel 1 (
    echo ❌ Failed to start application. Check the logs for details.
    goto end
)

echo.
echo ✅ Application started successfully!
echo.
echo 🌐 Access your application:
echo    Frontend: http://localhost:3000
echo    API:      http://localhost:8081
echo    Swagger:  http://localhost:8081/swagger
echo.
echo 📊 Check status: docker-run.bat status
echo 📝 View logs:    docker-run.bat logs
goto end

:stop
echo 🔴 Stopping Contact Agenda Application...
docker-compose -f docker-compose-fullstack.yml down

if errorlevel 1 (
    echo ❌ Failed to stop application.
    goto end
)

echo ✅ Application stopped successfully!
goto end

:restart
echo 🔄 Restarting Contact Agenda Application...
call :stop
echo.
call :start
goto end

:logs
echo 📝 Viewing application logs...
echo Press Ctrl+C to exit log view
echo.
docker-compose -f docker-compose-fullstack.yml logs -f
goto end

:status
echo 📊 Application Status:
echo =====================

REM Check if containers are running
docker-compose -f docker-compose-fullstack.yml ps | findstr "Up" >nul 2>&1

if errorlevel 1 (
    echo 🔴 Application is STOPPED
    echo.
    echo Start the application with: docker-run.bat start
    goto end
)

echo 🟢 Application is RUNNING
echo.
docker-compose -f docker-compose-fullstack.yml ps
echo.
echo 🌐 Application URLs:
echo    Frontend: http://localhost:3000
echo    API:      http://localhost:8081
goto end

:clean
echo 🧹 Cleaning up Docker resources...

REM Stop the application first
docker-compose -f docker-compose-fullstack.yml down

REM Remove unused containers, networks, images, and build cache
docker system prune -f

REM Remove build cache
docker builder prune -f

echo ✅ Docker cleanup completed!
echo 💾 Space freed up. You can check with: docker system df
goto end

:rebuild
echo 🔨 Rebuilding Contact Agenda Application...

REM Stop application
docker-compose -f docker-compose-fullstack.yml down

REM Rebuild images
docker-compose -f docker-compose-fullstack.yml build --no-cache

REM Start application
docker-compose -f docker-compose-fullstack.yml up -d

if errorlevel 1 (
    echo ❌ Failed to rebuild application.
    goto end
)

echo ✅ Application rebuilt and started successfully!
echo.
echo 🌐 Access your application:
echo    Frontend: http://localhost:3000
echo    API:      http://localhost:8081
goto end

:end
