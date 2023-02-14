#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Identity/ShopFComputerBackEnd.Identity.Api/ShopFComputerBackEnd.Identity.Api.csproj", "Services/Identity/ShopFComputerBackEnd.Identity.Api/"]
COPY ["Services/Identity/ShopFComputerBackEnd.Identity.Infrastructure/ShopFComputerBackEnd.Identity.Infrastructure.csproj", "Services/Identity/ShopFComputerBackEnd.Identity.Infrastructure/"]
COPY ["Services/Notification/ShopFComputerBackEnd.Notification.Shared/ShopFComputerBackEnd.Notification.Shared.csproj", "Services/Notification/ShopFComputerBackEnd.Notification.Shared/"]
COPY ["Services/Notification/ShopFComputerBackEnd.Notification.Domain/ShopFComputerBackEnd.Notification.Domain.csproj", "Services/Notification/ShopFComputerBackEnd.Notification.Domain/"]
COPY ["Services/Identity/ShopFComputerBackEnd.Identity.Data/ShopFComputerBackEnd.Identity.Data.csproj", "Services/Identity/ShopFComputerBackEnd.Identity.Data/"]
COPY ["Services/Identity/ShopFComputerBackEnd.Identity.Domain/ShopFComputerBackEnd.Identity.Domain.csproj", "Services/Identity/ShopFComputerBackEnd.Identity.Domain/"]
COPY ["Services/Profile/ShopFComputerBackEnd.Profile.Shared/ShopFComputerBackEnd.Profile.Shared.csproj", "Services/Profile/ShopFComputerBackEnd.Profile.Shared/"]
COPY ["Libraries/ShopFComputerBackEnd.Core.Authentication.Shared/ShopFComputerBackEnd.Core.Authentication.Shared.csproj", "Libraries/ShopFComputerBackEnd.Core.Authentication.Shared/"]
COPY ["Services/Identity/ShopFComputerBackEnd.Identity.Shared/ShopFComputerBackEnd.Identity.Shared.csproj", "Services/Identity/ShopFComputerBackEnd.Identity.Shared/"]
COPY ["Libraries/ShopFComputerBackEnd.Core.Authentication/ShopFComputerBackEnd.Core.Authentication.csproj", "Libraries/ShopFComputerBackEnd.Core.Authentication/"]
RUN dotnet restore "Services/Identity/ShopFComputerBackEnd.Identity.Api/ShopFComputerBackEnd.Identity.Api.csproj"
COPY . .
WORKDIR "/src/Services/Identity/ShopFComputerBackEnd.Identity.Api"
RUN dotnet build "ShopFComputerBackEnd.Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShopFComputerBackEnd.Identity.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopFComputerBackEnd.Identity.Api.dll"]