# ------------------------
# STAGE 1: BUILD
# ------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj and restore any dependencies (via dotnet restore)
COPY *.csproj ./
RUN dotnet restore

# Copy the entire project and build the app
COPY . .
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ------------------------
# STAGE 2: RUNTIME
# ------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the built files from the build stage
COPY --from=build /app/publish .

# Expose a new port
EXPOSE 8085

# Run the application
ENTRYPOINT ["dotnet", "api.dll"]