FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FIDS.Backend/FIDS.Backend.csproj", "FIDS.Backend/"]
RUN dotnet restore "FIDS.Backend/FIDS.Backend.csproj"
COPY . .
WORKDIR "/src/FIDS.Backend"
RUN dotnet build "FIDS.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FIDS.Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIDS.Backend.dll"]
