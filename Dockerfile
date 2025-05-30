# Etapa 1 - build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia tudo e restaura os pacotes
COPY . . 
WORKDIR /app/src/Pokedex.API
RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

# Etapa 2 - runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "Pokedex.API.dll"]
