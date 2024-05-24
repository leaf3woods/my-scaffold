#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MyScaffold.WebApi/MyScaffold.WebApi.csproj", "src/MyScaffold.WebApi/"]
COPY ["src/MyScaffold.Application/MyScaffold.Application.csproj", "src/MyScaffold.Application/"]
COPY ["src/MyScaffold.Core/MyScaffold.Core.csproj", "src/MyScaffold.Core/"]
COPY ["src/MyScaffold.Domain/MyScaffold.Domain.csproj", "src/MyScaffold.Domain/"]
COPY ["src/MyScaffold.Infrastructure/MyScaffold.Infrastructure.csproj", "src/MyScaffold.Infrastructure/"]
RUN dotnet restore "./src/MyScaffold.WebApi/MyScaffold.WebApi.csproj"
COPY . .
WORKDIR "/src/src/MyScaffold.WebApi"
RUN dotnet build "./MyScaffold.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyScaffold.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyScaffold.WebApi.dll"]