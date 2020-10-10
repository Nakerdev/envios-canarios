#Builder
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder
WORKDIR /api

COPY *.csproj ./
RUN dotnet restore

RUN dotnet publish -c Release -o WebAppApi

#Server
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as server
WORKDIR /api

COPY --from=builder /api/WebAppApi .

ENTRYPOINT ["dotnet", "aspnetapp.dll"]


