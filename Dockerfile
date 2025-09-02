# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj(s) first to leverage docker layer caching
COPY *.csproj ./
RUN dotnet restore

# copy the rest and publish
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app

# copy published app
COPY --from=build /app/publish ./

# copy your quotes file into the image (if it's not already included by the publish step)
# (If ShipTVQuotes.txt is in the project root, the previous COPY is enough. This is just explicit:)
# COPY ShipTVQuotes.txt ./ 

# run
# DISCORD_TOKEN will come from the platform env vars, not hardcoded
ENTRYPOINT ["dotnet", "ShipTVBot.dll"]
