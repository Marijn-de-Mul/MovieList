FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8079
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8079;http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ML.API/ML.API.csproj", "ML.API/"]
COPY ["ML.DAL/ML.DAL.csproj", "ML.DAL/"]
COPY ["ML.SAL/ML.SAL.csproj", "ML.SAL/"]
RUN dotnet restore "ML.API/ML.API.csproj"
COPY . .
WORKDIR "/src/ML.API"
RUN dotnet build "ML.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ML.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ML.API.dll"]
