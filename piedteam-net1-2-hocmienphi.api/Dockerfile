FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["piedteam-net1-2-hocmienphi.api/piedteam-net1-2-hocmienphi.api.csproj", "piedteam-net1-2-hocmienphi.api/"]
COPY ["piedteam-net1-2-hocmienphi.service/piedteam-net1-2-hocmienphi.service.csproj", "piedteam-net1-2-hocmienphi.service/"]
COPY ["piedteam-net1-2-hocmienphi.repository/piedteam-net1-2-hocmienphi.repository.csproj", "piedteam-net1-2-hocmienphi.repository/"]
RUN dotnet restore "piedteam-net1-2-hocmienphi.api/piedteam-net1-2-hocmienphi.api.csproj"
COPY . .
WORKDIR "/src/piedteam-net1-2-hocmienphi.api"
RUN dotnet build "./piedteam-net1-2-hocmienphi.api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./piedteam-net1-2-hocmienphi.api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "piedteam-net1-2-hocmienphi.api.dll"]
