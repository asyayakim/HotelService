FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HotelService.Api/HotelService.Api.csproj", "HotelService.Api/"]
COPY ["HotelService.Db/HotelService.Db.csproj", "HotelService.Db/"]
COPY ["HotelService.Logic/HotelService.Logic.csproj", "HotelService.Logic/"]
RUN dotnet restore "HotelService.Api/HotelService.Api.csproj"
COPY . .
WORKDIR "/src/HotelService.Api"
RUN dotnet build "./HotelService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HotelService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelService.Api.dll"]
