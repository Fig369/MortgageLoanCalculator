FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MortgageLoanCalculator/MortgageLoanCalculator.csproj", "MortgageLoanCalculator/"]
RUN dotnet restore "MortgageLoanCalculator/MortgageLoanCalculator.csproj"
COPY . .
WORKDIR "/src/MortgageLoanCalculator"
RUN dotnet build "MortgageLoanCalculator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MortgageLoanCalculator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MortgageLoanCalculator.dll"]
