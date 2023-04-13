FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CopyPlayerService/CopyPlayerService.csproj", "CopyPlayerService/"]
RUN dotnet restore "CopyPlayerService/CopyPlayerService.csproj"
COPY . .
WORKDIR "/src/CopyPlayerService"
RUN dotnet build "CopyPlayerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CopyPlayerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CopyPlayerService.dll"]
