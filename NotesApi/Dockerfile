# runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# копируем sln и все csproj (для кэширования restore)
COPY ["NotesApi.sln", "./"]
COPY ["NotesApi.Domain/NotesApi.Domain.csproj", "NotesApi.Domain/"]
COPY ["NotesApi.Application/NotesApi.Application.csproj", "NotesApi.Application/"]
COPY ["NotesApi.Infrastructure/NotesApi.Infrastructure.csproj", "NotesApi.Infrastructure/"]
COPY ["NotesApi/NotesApi.csproj", "NotesApi/"]

# restore
RUN dotnet restore "NotesApi/NotesApi.csproj"

# копируем весь код (включая папку Migrations!)
COPY . .

# publish API
RUN dotnet publish "NotesApi/NotesApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
# Убрать дублирующую строку копирования Infrastructure.dll - она уже есть в publish
ENTRYPOINT ["dotnet", "NotesApi.dll"]
