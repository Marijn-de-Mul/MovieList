FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN apt-get update && apt-get install -y openssl

WORKDIR /src

COPY ML.Backend.sln . 
COPY ML.DAL/ML.DAL.csproj ML.DAL/
COPY ML.SAL/ML.SAL.csproj ML.SAL/
COPY ML.API/ML.API.csproj ML.API/

COPY ML.DAL/ ML.DAL/
COPY ML.SAL/ ML.SAL/
COPY ML.API/ ML.API/

RUN dotnet restore "ML.Backend.sln"
RUN dotnet clean "ML.Backend.sln" && dotnet build "ML.Backend.sln" -c Release -o /app/build
RUN dotnet publish "ML.Backend.sln" -c Release -o /app/publish

# Remove unnecessary or conflicting files
RUN rm -f /app/publish/ML.API

RUN mkdir -p /etc/ssl/private /etc/ssl/certs && \
    openssl genpkey -algorithm RSA -out /etc/ssl/private/mycertificate.key -pkeyopt rsa_keygen_bits:2048 && \
    openssl req -new -key /etc/ssl/private/mycertificate.key -out /etc/ssl/private/mycertificate.csr -subj "/CN=localhost" && \
    openssl x509 -req -in /etc/ssl/private/mycertificate.csr -signkey /etc/ssl/private/mycertificate.key -out /etc/ssl/certs/mycertificate.crt

EXPOSE 8079
EXPOSE 8080

WORKDIR /app/publish

ENTRYPOINT ["dotnet", "ML.API.dll"]
