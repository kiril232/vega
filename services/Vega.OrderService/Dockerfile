# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Vega.OrderService.csproj . 
RUN dotnet restore 

COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# serve
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR app/

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Vega.OrderService.dll"]