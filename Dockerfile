# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj first to leverage caching
COPY *.csproj ./
RUN dotnet restore

# copy everything and publish
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /app
COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "ShipTVBot.dll"]
