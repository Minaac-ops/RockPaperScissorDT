FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GameMasterService/GameMasterService.csproj", "GameMasterService/"]
RUN dotnet restore "GameMasterService/GameMasterService.csproj"
COPY . .
WORKDIR "/src/GameMasterService"
RUN dotnet build "GameMasterService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameMasterService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameMasterService.dll"]
