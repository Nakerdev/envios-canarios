#Builder
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS builder

COPY ./WebAppApi/. /api
RUN dotnet restore ./api/WebAppApi.csproj
RUN dotnet build ./api/WebAppApi.csproj -c Release -o ./build

RUN dotnet publish ./api/WebAppApi.csproj -c Release -o ./publish

#Server
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 as server
EXPOSE 5000

COPY --from=builder /publish .

ENTRYPOINT ["dotnet", "WebAppApi.dll"]


