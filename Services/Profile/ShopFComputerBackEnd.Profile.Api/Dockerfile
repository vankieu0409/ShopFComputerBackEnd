#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Profile/ShopFComputerBackEnd.Profile.Api/ShopFComputerBackEnd.Profile.Api.csproj", "Services/Profile/ShopFComputerBackEnd.Profile.Api/"]
COPY ["Services/Profile/ShopFComputerBackEnd.Profile.Shared/ShopFComputerBackEnd.Profile.Shared.csproj", "Services/Profile/ShopFComputerBackEnd.Profile.Shared/"]
COPY ["Services/Profile/ShopFComputerBackEnd.Profile.Infrastructure/ShopFComputerBackEnd.Profile.Infrastructure.csproj", "Services/Profile/ShopFComputerBackEnd.Profile.Infrastructure/"]
COPY ["Services/Identity/ShopFComputerBackEnd.Identity.Shared/ShopFComputerBackEnd.Identity.Shared.csproj", "Services/Identity/ShopFComputerBackEnd.Identity.Shared/"]
COPY ["Services/Profile/ShopFComputerBackEnd.Profile.Data/ShopFComputerBackEnd.Profile.Data.csproj", "Services/Profile/ShopFComputerBackEnd.Profile.Data/"]
COPY ["Services/Profile/ShopFComputerBackEnd.Profile.Domain/ShopFComputerBackEnd.Profile.Domain.csproj", "Services/Profile/ShopFComputerBackEnd.Profile.Domain/"]
COPY ["Libraries/ShopFComputerBackEnd.Core.Authentication/ShopFComputerBackEnd.Core.Authentication.csproj", "Libraries/ShopFComputerBackEnd.Core.Authentication/"]
COPY ["Libraries/ShopFComputerBackEnd.Core.Authentication.Shared/ShopFComputerBackEnd.Core.Authentication.Shared.csproj", "Libraries/ShopFComputerBackEnd.Core.Authentication.Shared/"]
RUN dotnet restore "Services/Profile/ShopFComputerBackEnd.Profile.Api/ShopFComputerBackEnd.Profile.Api.csproj"
COPY . .
WORKDIR "/src/Services/Profile/ShopFComputerBackEnd.Profile.Api"
RUN dotnet build "ShopFComputerBackEnd.Profile.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShopFComputerBackEnd.Profile.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopFComputerBackEnd.Profile.Api.dll"]