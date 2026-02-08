FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
WORKDIR /src

EXPOSE 8080
CMD ["bash", "-lc", "dotnet restore ./OrdersService.Api.csproj && dotnet watch --project ./OrdersService.Api.csproj run --urls http://0.0.0.0:8080"]