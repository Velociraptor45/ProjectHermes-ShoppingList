FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster-arm32v7 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "ShoppingList.Api.WebApp/ShoppingList.Api.WebApp.csproj"
COPY . .
RUN dotnet build "ShoppingList.Api.WebApp/ShoppingList.Api.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShoppingList.Api.WebApp/ShoppingList.Api.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ssl/ ssl/
ENTRYPOINT ["dotnet", "ShoppingList.Api.WebApp.dll"]