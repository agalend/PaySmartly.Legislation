# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY PaySmartly.Legislation/*.csproj ./PaySmartly.Legislation/
RUN dotnet restore

# copy everything else and build app
COPY PaySmartly.Legislation/. ./PaySmartly.Legislation/
WORKDIR /source/PaySmartly.Legislation
RUN dotnet publish -c release -o /PaySmartly.Legislation --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

LABEL author="Stefan Bozov"

ENV ASPNETCORE_URLS=http://*:9090
ENV ASPNETCORE_ENVIRONMENT="production"

EXPOSE 9090

WORKDIR /PaySmartly.Legislation
COPY --from=build /PaySmartly.Legislation ./
ENTRYPOINT ["dotnet", "PaySmartly.Legislation.dll"]