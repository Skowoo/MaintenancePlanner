#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Identity/IdentityService/IdentityServiceAPI.csproj", "src/Services/Identity/IdentityService/"]
COPY ["src/Auxiliaries/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "src/Auxiliaries/EventBus/EventBusRabbitMQ/"]
COPY ["src/Auxiliaries/EventBus/EventBus/EventBus.csproj", "src/Auxiliaries/EventBus/EventBus/"]
RUN dotnet restore "./src/Services/Identity/IdentityService/IdentityServiceAPI.csproj"
COPY . .
WORKDIR "/src/src/Services/Identity/IdentityService"
RUN dotnet build "./IdentityServiceAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IdentityServiceAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityServiceAPI.dll"]