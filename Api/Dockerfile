# BUILD STAGE
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /Api

# Dependencies
COPY Api.csproj ./
RUN dotnet restore

# App
COPY . ./

# Build + Publish
RUN dotnet publish -c Release -o ./publish /p:UseAppHost=false

# RUNTIME STAGE, not alpine because of missing dependencies
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /Api

COPY --from=build /Api/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_HTTP_PORTS=5033

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

USER app

HEALTHCHECK --interval=30s --timeout=3s --retries=3 --start-period=20s \
    CMD curl -f http://localhost:5033/health || exit 1

ENTRYPOINT [ "dotnet", "Api.dll" ]