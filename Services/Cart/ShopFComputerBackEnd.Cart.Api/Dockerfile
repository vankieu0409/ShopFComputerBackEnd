#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Cart/ShopFComputerBackEnd.Cart.Api/ShopFComputerBackEnd.Cart.Api.csproj", "Services/Cart/ShopFComputerBackEnd.Cart.Api/"]
COPY ["Services/Cart/ShopFComputerBackEnd.Cart.Infrastructure/ShopFComputerBackEnd.Cart.Infrastructure.csproj", "Services/Cart/ShopFComputerBackEnd.Cart.Infrastructure/"]
COPY ["Services/Cart/ShopFComputerBackEnd.Cart.Data/ShopFComputerBackEnd.Cart.Data.csproj", "Services/Cart/ShopFComputerBackEnd.Cart.Data/"]
COPY ["Services/Cart/ShopFComputerBackEnd.Cart.Domain/ShopFComputerBackEnd.Cart.Domain.csproj", "Services/Cart/ShopFComputerBackEnd.Cart.Domain/"]
COPY ["Libraries/ShopFComputerBackEnd.Core.Authentication/ShopFComputerBackEnd.Core.Authentication.csproj", "Libraries/ShopFComputerBackEnd.Core.Authentication/"]
COPY ["Libraries/ShopFComputerBackEnd.Core.Authentication.Shared/ShopFComputerBackEnd.Core.Authentication.Shared.csproj", "Libraries/ShopFComputerBackEnd.Core.Authentication.Shared/"]
RUN dotnet restore "Services/Cart/ShopFComputerBackEnd.Cart.Api/ShopFComputerBackEnd.Cart.Api.csproj"
COPY . .
WORKDIR "/src/Services/Cart/ShopFComputerBackEnd.Cart.Api"
RUN dotnet build "ShopFComputerBackEnd.Cart.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShopFComputerBackEnd.Cart.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopFComputerBackEnd.Cart.Api.dll"]