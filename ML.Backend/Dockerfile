FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN apt-get update && apt-get install -y openssl

WORKDIR /src

# Copy solution and project files
COPY ML.Backend.sln . 
COPY ML.DAL/ML.DAL.csproj ML.DAL/
COPY ML.SAL/ML.SAL.csproj ML.SAL/
COPY ML.API/ML.API.csproj ML.API/

# Copy all source code
COPY ML.DAL/ ML.DAL/
COPY ML.SAL/ ML.SAL/
COPY ML.API/ ML.API/

# Restore, build, and publish
RUN dotnet restore "ML.Backend.sln"
RUN dotnet build "ML.Backend.sln" -c Release -o /app/build
RUN dotnet publish "ML.Backend.sln" -c Release -o /app/publish

# Generate self-signed certificate
RUN mkdir -p /etc/ssl/private /etc/ssl/certs && \
    openssl genpkey -algorithm RSA -out /etc/ssl/private/mycertificate.key -pkeyopt rsa_keygen_bits:2048 && \
    openssl req -new -key /etc/ssl/private/mycertificate.key -out /etc/ssl/private/mycertificate.csr -subj "/CN=localhost" && \
    openssl x509 -req -in /etc/ssl/private/mycertificate.csr -signkey /etc/ssl/private/mycertificate.key -out /etc/ssl/certs/mycertificate.crt

# Expose ports
EXPOSE 8079
EXPOSE 8080

# Set working directory and entrypoint
WORKDIR /app/publish
ENTRYPOINT ["ML.API.dll"]
