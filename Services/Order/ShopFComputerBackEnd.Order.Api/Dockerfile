#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Order/ShopFComputerBackEnd.Order.Api/ShopFComputerBackEnd.Order.Api.csproj", "Services/Order/ShopFComputerBackEnd.Order.Api/"]
COPY ["Services/Order/ShopFComputerBackEnd.Order.Infrastructure/ShopFComputerBackEnd.Order.Infrastructure.csproj", "Services/Order/ShopFComputerBackEnd.Order.Infrastructure/"]
COPY ["Services/Order/ShopFComputerBackEnd.Order.Data/ShopFComputerBackEnd.Order.Data.csproj", "Services/Order/ShopFComputerBackEnd.Order.Data/"]
COPY ["Services/Order/ShopFComputerBackEnd.Order.Domain/ShopFComputerBackEnd.Order.Domain.csproj", "Services/Order/ShopFComputerBackEnd.Order.Domain/"]
RUN dotnet restore "Services/Order/ShopFComputerBackEnd.Order.Api/ShopFComputerBackEnd.Order.Api.csproj"
COPY . .
WORKDIR "/src/Services/Order/ShopFComputerBackEnd.Order.Api"
RUN dotnet build "ShopFComputerBackEnd.Order.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShopFComputerBackEnd.Order.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopFComputerBackEnd.Order.Api.dll"]