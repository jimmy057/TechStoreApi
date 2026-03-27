# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiamos todos los archivos del repositorio al contenedor
COPY . .

# Restauramos los paquetes
RUN dotnet restore

# Publicamos la aplicación
# (Asegúrate de que el nombre del proyecto coincida con tu archivo .csproj)
RUN dotnet publish -c Release -o out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copiamos lo publicado de la etapa anterior
COPY --from=build /app/out .

# Configuramos el puerto para Render
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# El nombre del .dll debe ser el mismo que tu proyecto
ENTRYPOINT ["dotnet", "TechStoreApi.dll"]