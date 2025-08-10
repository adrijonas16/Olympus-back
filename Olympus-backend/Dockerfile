FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los proyectos .csproj
COPY Olympus-backend/CapaNegocio/CapaNegocio.csproj CapaNegocio/
COPY Olympus-backend/Olympus.API/Olympus.API.csproj Olympus.API/
COPY Olympus-backend/Olympus.DAL/CapaDatos.csproj Olympus.DAL/
COPY Olympus-backend/Olympus.Models/Modelos.csproj Olympus.Models/

# Restaurar el proyecto API principal
RUN dotnet restore Olympus.API/Olympus.API.csproj

# Copiar todo el código fuente de cada proyecto
COPY Olympus-backend/CapaNegocio/ CapaNegocio/
COPY Olympus-backend/Olympus.API/ Olympus.API/
COPY Olympus-backend/Olympus.DAL/ Olympus.DAL/
COPY Olympus-backend/Olympus.Models/ Olympus.Models/

# Publicar la app (API principal)
RUN dotnet publish Olympus.API/Olympus.API.csproj -c Release -o /app/publish

# Imagen runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Olympus.API.dll"]
