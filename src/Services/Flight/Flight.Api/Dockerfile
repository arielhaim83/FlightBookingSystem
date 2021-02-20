#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/Services/Flight/Flight.Api/Flight.Api.csproj", "src/Services/Flight/Flight.Api/"]
COPY ["src/Services/Flight/Flight.Domain/Flight.Domain.csproj", "src/Services/Flight/Flight.Domain/"]
RUN dotnet restore "src/Services/Flight/Flight.Api/Flight.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Flight/Flight.Api"
RUN dotnet build "Flight.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Flight.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Flight.Api.dll"]