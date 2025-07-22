# Multi-stage Dockerfile for Contact Agenda API
# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj files and restore dependencies
# This is done first to leverage Docker layer caching
COPY *.csproj ./
RUN dotnet restore

# Copy source code and build (exclude test project)
COPY Controllers/ ./Controllers/
COPY CQRS/ ./CQRS/
COPY DTOs/ ./DTOs/
COPY Interfaces/ ./Interfaces/
COPY Migrations/ ./Migrations/
COPY Models/ ./Models/
COPY Properties/ ./Properties/
COPY Repositories/ ./Repositories/
COPY Services/ ./Services/
COPY Validators/ ./Validators/
COPY Program.cs ./
COPY MappingProfile.cs ./
COPY appsettings*.json ./
RUN dotnet build -c Release --no-restore

# Publish the application
RUN dotnet publish -c Release --no-build -o /app/publish

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Create data directory for SQLite database
RUN mkdir -p /app/data && chown -R appuser:appuser /app/data

# Copy published application
COPY --from=build /app/publish .

# Copy database if it exists (for development)
RUN if [ -f /app/contacts.db ]; then cp /app/contacts.db ./data/; fi

# Set ownership
RUN chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Configure environment
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5000
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/contacts.db"

# Expose port
EXPOSE 5000

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:5000/swagger || exit 1

# Run the application
ENTRYPOINT ["dotnet", "ContactAgendaApi.dll"]
