#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FIDS.ArivalClient/FIDS.ArivalClient.csproj", "FIDS.ArivalClient/"]
COPY ["FIDS.Backend/FIDS.Backend.csproj", "FIDS.Backend/"]
RUN dotnet restore "FIDS.ArivalClient/FIDS.ArivalClient.csproj"
COPY . .
WORKDIR "/src/FIDS.ArivalClient"
RUN dotnet build "FIDS.ArivalClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FIDS.ArivalClient.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
RUN apt update -y && apt-get install -y libbrotli1 libmbedtls12 && apt-get clean
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIDS.ArivalClient.dll"]
