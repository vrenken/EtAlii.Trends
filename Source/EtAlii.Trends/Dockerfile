FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EtAlii.Trends/EtAlii.Trends.csproj", "EtAlii.Trends/"]
RUN dotnet restore "EtAlii.Trends/EtAlii.Trends.csproj"
COPY . .
WORKDIR "/src/EtAlii.Trends"
RUN dotnet build "EtAlii.Trends.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EtAlii.Trends.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EtAlii.Trends.dll"]
