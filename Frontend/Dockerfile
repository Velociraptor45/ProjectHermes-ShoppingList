################## BUILD ##################
FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS build
WORKDIR /src
COPY . .
RUN dotnet restore "ShoppingList.Frontend.WebApp/ShoppingList.Frontend.WebApp.csproj"
RUN dotnet build "ShoppingList.Frontend.WebApp/ShoppingList.Frontend.WebApp.csproj" -c Release -o /app/build

################# PUBLISH #################
FROM build AS publish
RUN dotnet publish "ShoppingList.Frontend.WebApp/ShoppingList.Frontend.WebApp.csproj" -c Release -o /app/publish

################## FINAL ##################
FROM nginxinc/nginx-unprivileged:1.25 AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish .
COPY Docker/nginx.conf /etc/nginx/nginx.conf

CMD ["nginx", "-g", "daemon off;"]