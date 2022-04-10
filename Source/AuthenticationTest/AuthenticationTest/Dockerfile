FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AuthenticationTest/AuthenticationTest/AuthenticationTest.csproj", "AuthenticationTest/"]
RUN dotnet restore "AuthenticationTest/AuthenticationTest/AuthenticationTest.csproj"
COPY . .
WORKDIR "/src/AuthenticationTest"
RUN dotnet build "AuthenticationTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthenticationTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthenticationTest.dll"]
