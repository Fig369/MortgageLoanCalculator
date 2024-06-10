# Use .NET 6.0 as the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use .NET 6.0 SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MortgageLoanCalculator/MortgageLoanCalculator.csproj", "MortgageLoanCalculator/"]
RUN dotnet restore "./MortgageLoanCalculator/MortgageLoanCalculator.csproj"
COPY . .
WORKDIR "/src/MortgageLoanCalculator"
RUN dotnet build "./MortgageLoanCalculator.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MortgageLoanCalculator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use the base image for the final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MortgageLoanCalculator.dll"]
