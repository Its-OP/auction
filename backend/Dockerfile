FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["h20auctions.csproj", "./"]
RUN dotnet restore "h20auctions.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "h20auctions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "h20auctions.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "h20auctions.dll"]
