# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["ServiceNotification.csproj", "./"]
RUN dotnet restore "ServiceNotification.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "ServiceNotification.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "ServiceNotification.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create non-root user
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "ServiceNotification.dll"]
