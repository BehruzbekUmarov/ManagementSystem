# Use ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use .NET SDK image to build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ManagementSystem.sln ./
COPY InnerSystem.Api/InnerSystem.Api.csproj ./InnerSystem.Api/
COPY InnerSystem.Identity/InnerSystem.Identity.csproj ./InnerSystem.Identity/

# Restore dependencies
RUN dotnet restore "ManagementSystem.sln"

# Copy the entire solution and build the API
COPY . .
WORKDIR /src/InnerSystem.Api
RUN dotnet publish -c Release -o /app/publish

# Final stage: runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InnerSystem.Api.dll"]
