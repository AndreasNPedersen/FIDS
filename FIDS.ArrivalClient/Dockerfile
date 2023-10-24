#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FIDS.ArrivalClient/FIDS.ArivalClient.csproj", "FIDS.ArrivalClient/"]
COPY ["FIDS.Backend/FIDS.Backend.csproj", "FIDS.Backend/"]
RUN dotnet restore "FIDS.ArrivalClient/FIDS.ArrivalClient.csproj"
COPY . .
WORKDIR "/src/FIDS.ArrivalClient"
RUN dotnet build "FIDS.ArrivalClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FIDS.ArrivalClient.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
RUN apt update -y && apt-get install -y libbrotli1 libmbedtls12 && apt-get clean
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIDS.ArrivalClient.dll"]
