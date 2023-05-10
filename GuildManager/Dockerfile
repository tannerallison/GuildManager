FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GuildManager/GuildManager.csproj", "GuildManager/"]
RUN dotnet restore "GuildManager/GuildManager.csproj"
COPY . .
WORKDIR "/src/GuildManager"
RUN dotnet build "GuildManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GuildManager.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GuildManager.dll"]
