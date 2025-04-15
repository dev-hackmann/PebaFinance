# Use the official .NET SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and restore dependencies
COPY PebaFinance.sln ./
COPY src/PebaFinance.Api/PebaFinance.Api.csproj src/PebaFinance.Api/
COPY src/PebaFinance.Application/PebaFinance.Application.csproj src/PebaFinance.Application/
COPY src/PebaFinance.Domain/PebaFinance.Domain.csproj src/PebaFinance.Domain/
COPY src/PebaFinance.Infrastructure/PebaFinance.Infrastructure.csproj src/PebaFinance.Infrastructure/
RUN dotnet restore

# Copy the entire source code and build the application
COPY . .
WORKDIR /app/src/PebaFinance.Api
RUN dotnet publish -c Release -o /out

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

# Expose the port the app runs on
EXPOSE 8080

# Set the entry point for the container
ENTRYPOINT ["dotnet", "PebaFinance.Api.dll"]