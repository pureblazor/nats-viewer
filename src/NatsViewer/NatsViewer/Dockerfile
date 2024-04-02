FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BlazorApp1/BlazorApp1/BlazorApp1.csproj", "BlazorApp1/BlazorApp1/"]
COPY ["BlazorApp1/BlazorApp1.Client/BlazorApp1.Client.csproj", "BlazorApp1/BlazorApp1.Client/"]
RUN dotnet restore "BlazorApp1/BlazorApp1/BlazorApp1.csproj"
COPY . .
WORKDIR "/src/BlazorApp1/BlazorApp1"
RUN dotnet build "BlazorApp1.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BlazorApp1.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorApp1.dll"]
