# =========================
# Build and publish
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Get latest Node.js to build Angular later
RUN apt-get update && apt-get install -y curl \
    && curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs \
    && npm install -g npm@11 \
    && rm -rf /var/lib/apt/lists/*

# Copy project files
COPY src/api/Mybad.API/*.csproj src/api/Mybad.API/
COPY src/api/Mybad.Core/*.csproj src/api/Mybad.Core/
COPY src/api/Mybad.Core.Services/Mybad.Services.OpenDota/*.csproj src/api/Mybad.Core.Services/Mybad.Services.OpenDota/
COPY src/api/Mybad.Storage/*.csproj src/api/Mybad.Storage/
COPY src/ui/front/Mybad.AngularFront/*.esproj src/ui/front/Mybad.AngularFront/

# Copy Angular package files
COPY src/ui/front/Mybad.AngularFront/package*.json src/ui/front/Mybad.AngularFront/

# Restore dependencies
RUN dotnet restore src/api/Mybad.API/Mybad.API.csproj

# Install Angular dependencies (Cached)
# Using 'npm ci' is for automated builds
WORKDIR /source/src/ui/front/Mybad.AngularFront
RUN npm ci

# Return to workdir
WORKDIR /source

# Copy full source
COPY src/ ./src/

# Publish (this runs Angular build internally)
RUN dotnet publish src/api/Mybad.API/Mybad.API.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# =========================
# Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app

# Expose port 8080 to be available
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Mybad.API.dll"]