FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY TPI-GESTION-HOGAR.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish TPI-GESTION-HOGAR.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "TPI-GESTION-HOGAR.dll"]