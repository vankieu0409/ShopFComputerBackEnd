#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Product/ShopFComputerBackEnd.Product.Api/ShopFComputerBackEnd.Product.Api.csproj", "Services/Product/ShopFComputerBackEnd.Product.Api/"]
COPY ["Services/Product/ShopFComputerBackEnd.Product.Infrastructure/ShopFComputerBackEnd.Product.Infrastructure.csproj", "Services/Product/ShopFComputerBackEnd.Product.Infrastructure/"]
COPY ["Services/Product/ShopFComputerBackEnd.Product.Data/ShopFComputerBackEnd.Product.Data.csproj", "Services/Product/ShopFComputerBackEnd.Product.Data/"]
COPY ["Services/Product/ShopFComputerBackEnd.Product.Domain/ShopFComputerBackEnd.Product.Domain.csproj", "Services/Product/ShopFComputerBackEnd.Product.Domain/"]
RUN dotnet restore "Services/Product/ShopFComputerBackEnd.Product.Api/ShopFComputerBackEnd.Product.Api.csproj"
COPY . .
WORKDIR "/src/Services/Product/ShopFComputerBackEnd.Product.Api"
RUN dotnet build "ShopFComputerBackEnd.Product.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShopFComputerBackEnd.Product.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopFComputerBackEnd.Product.Api.dll"]