FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src

COPY ["Rat.Api/Rat.Api.csproj", "Rat.Api/"]
COPY ["Rat.Core/Rat.Core.csproj", "Rat.Core/"]
COPY ["Rat.DataAccess/Rat.DataAccess.csproj", "Rat.DataAccess/"]
COPY ["Rat.Data/Rat.Data.csproj", "Rat.Data/"]
RUN dotnet restore "Rat.Api/Rat.Api.csproj"

COPY . .
WORKDIR "/src/Rat.Api"
RUN dotnet build "Rat.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rat.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rat.Api.dll"]
