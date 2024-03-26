#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER root
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CozyMarinaBot/CozyMarinaBot.csproj", "CozyMarinaBot/"]
RUN dotnet restore "./CozyMarinaBot/CozyMarinaBot.csproj"
COPY . .
WORKDIR "/src/CozyMarinaBot"
RUN dotnet build "./CozyMarinaBot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CozyMarinaBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
RUN chmod 666 CozyMarinaBot.db

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CozyMarinaBot.dll"]