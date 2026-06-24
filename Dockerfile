FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_HTTP_PORTS=5033


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#Optimized build mode for production
ARG BUILD_CONFIGURATION=Release 
WORKDIR /build

COPY ["Broke-Manager.sln", "./"]
COPY ["src/Api/Api.csproj", "src/Api/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Contracts/Contracts.csproj", "src/Contracts/"]

RUN dotnet restore "src/Api/Api.csproj"

COPY . .

RUN dotnet publish "/build/src/Api/Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .

USER app

HEALTHCHECK --interval=30s --timeout=3s --retries=3 --start-period=20s \
    CMD curl -f http://localhost:5033/health || exit 1

ENTRYPOINT ["dotnet", "Api.dll"]