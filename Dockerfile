# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar archivos y restaurar
COPY *.sln .
COPY TechStoreApi/*.csproj ./TechStoreApi/
RUN dotnet restore

# Copiar todo y publicar
COPY . .
WORKDIR /app/TechStoreApi
RUN dotnet publish -c Release -o out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/TechStoreApi/out .

# Render usa el puerto 8080 por defecto para ASP.NET Core
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TechStoreApi.dll"]