# Stage 1: Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Install required libraries for DinkToPdf
RUN apt-get update \
    && apt-get install -y --no-install-recommends apt-utils \
    && apt-get install -y --no-install-recommends \
        libgdiplus \
        libc6 \
        libx11-6 \
        libxext6 \
        libxrender1 \
        libxfixes3 \
        libxrandr2 \
        libxinerama1 \
        libxcursor1 \
        libxcomposite1 \
        libfontconfig1 \
        libfreetype6 \
        libglib2.0-0 \
        libjpeg62-turbo \
        libpng16-16 \
        xvfb \
    && rm -rf /var/lib/apt/lists/*



# Switch to non-root user
USER app

# Stage 2: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files for restore
COPY ["TravelEase.API/TravelEase.API.csproj", "TravelEase.API/"]
COPY ["TravelEase.Application/TravelEase.Application.csproj", "TravelEase.Application/"]
COPY ["TravelEase.Domain/TravelEase.Domain.csproj", "TravelEase.Domain/"]
COPY ["TravelEase.Infrastructure/TravelEase.Infrastructure.csproj", "TravelEase.Infrastructure/"]

RUN dotnet restore "./TravelEase.API/TravelEase.API.csproj"

# Copy all source code including DinkToPdf folder
COPY . .

WORKDIR "/src/TravelEase.API"
RUN dotnet build "./TravelEase.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TravelEase.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Include DinkToPdf folder in publish output with proper ownership
COPY --from=build --chown=app:app /src/TravelEase.Infrastructure/DinkToPdf /app/publish/DinkToPdf

# Stage 4: Final runtime image
FROM base AS final
WORKDIR /app

# Temporarily switch to root for privileged actions
USER root

# Copy published files including DinkToPdf
COPY --from=publish /app/publish .

# Add DinkToPdf shared library to system library path
RUN cp /app/DinkToPdf/libwkhtmltox/libwkhtmltox.so /usr/lib/libwkhtmltox.so \
    && chown app:app /usr/lib/libwkhtmltox.so

# Switch back to non-root user for security
USER app

ENTRYPOINT ["dotnet", "TravelEase.API.dll"]
