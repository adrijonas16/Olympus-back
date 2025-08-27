# ---- Etapa 1: Compilación (Build) ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copiar solución y proyectos
COPY *.sln .
COPY CapaNegocio/*.csproj ./CapaNegocio/
COPY Olympus.API/*.csproj ./Olympus.API/
COPY Olympus.DAL/*.csproj ./Olympus.DAL/
COPY Olympus.Models/*.csproj ./Olympus.Models/

# 2. Restaurar dependencias
RUN dotnet restore "Olympus-backend.sln"

# 3. Copiar el código
COPY . .

# 4. Publicar
WORKDIR "/src/Olympus.API"
RUN dotnet publish -c Release -o /app/publish

# ---- Etapa 2: Final (Producción) ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Instalar librerías necesarias para SQL Server
RUN apt-get update && \
    apt-get install -y \
        libgssapi-krb5-2 \
        unixodbc \
        curl \
        gnupg \
        wget \
        ca-certificates && \
    # Descargar e importar la clave GPG de Microsoft de forma más robusta
    wget -qO - https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor -o /usr/share/keyrings/microsoft-archive-keyring.gpg && \
    # Agregar el repositorio de Microsoft con la clave correcta
    echo "deb [arch=amd64 signed-by=/usr/share/keyrings/microsoft-archive-keyring.gpg] https://packages.microsoft.com/debian/12/prod bookworm main" > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y msodbcsql18 && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Exponer puerto
EXPOSE 8080

# Copiar publicación
COPY --from=build /app/publish .

# Entrypoint
ENTRYPOINT ["dotnet", "Olympus.API.dll"]
