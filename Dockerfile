# Base image for runtime (more complete variant avoids common native crashes)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the full source to avoid restore/build issues
COPY . .

# Restore dependencies using the solution
RUN dotnet restore "ManagementSystem.sln"

# Publish the API project
WORKDIR /src/InnerSystem.Api
RUN dotnet publish -c Release -o /app/publish

# Final stage: copy build output to runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Set the entrypoint to run your API
ENTRYPOINT ["dotnet", "InnerSystem.Api.dll"]
