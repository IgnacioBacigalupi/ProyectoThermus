FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Thermus.Api.csproj ./
RUN dotnet restore ./Thermus.Api.csproj

COPY . .
RUN dotnet publish ./Thermus.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:5073
EXPOSE 5073

ENTRYPOINT ["dotnet", "Thermus.Api.dll"]