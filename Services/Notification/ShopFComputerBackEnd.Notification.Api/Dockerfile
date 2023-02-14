#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Notification/ShopFComputerBackEnd.Notification.Api/ShopFComputerBackEnd.Notification.Api.csproj", "Services/Notification/ShopFComputerBackEnd.Notification.Api/"]
COPY ["Services/Notification/ShopFComputerBackEnd.Notification.Infrastructure/ShopFComputerBackEnd.Notification.Infrastructure.csproj", "Services/Notification/ShopFComputerBackEnd.Notification.Infrastructure/"]
COPY ["Services/Identity/ShopFComputerBackEnd.Identity.Shared/ShopFComputerBackEnd.Identity.Shared.csproj", "Services/Identity/ShopFComputerBackEnd.Identity.Shared/"]
COPY ["Services/Profile/ShopFComputerBackEnd.Profile.Shared/ShopFComputerBackEnd.Profile.Shared.csproj", "Services/Profile/ShopFComputerBackEnd.Profile.Shared/"]
COPY ["Services/Notification/ShopFComputerBackEnd.Notification.Shared/ShopFComputerBackEnd.Notification.Shared.csproj", "Services/Notification/ShopFComputerBackEnd.Notification.Shared/"]
COPY ["Services/Notification/ShopFComputerBackEnd.Notification.Domain/ShopFComputerBackEnd.Notification.Domain.csproj", "Services/Notification/ShopFComputerBackEnd.Notification.Domain/"]
COPY ["Services/Notification/ShopFComputerBackEnd.Notification.Data/ShopFComputerBackEnd.Notification.Data.csproj", "Services/Notification/ShopFComputerBackEnd.Notification.Data/"]
COPY ["Libraries/ShopFComputerBackEnd.Core.Authentication/ShopFComputerBackEnd.Core.Authentication.csproj", "Libraries/ShopFComputerBackEnd.Core.Authentication/"]
COPY ["Libraries/ShopFComputerBackEnd.Core.Authentication.Shared/ShopFComputerBackEnd.Core.Authentication.Shared.csproj", "Libraries/ShopFComputerBackEnd.Core.Authentication.Shared/"]
RUN dotnet restore "Services/Notification/ShopFComputerBackEnd.Notification.Api/ShopFComputerBackEnd.Notification.Api.csproj"
COPY . .
WORKDIR "/src/Services/Notification/ShopFComputerBackEnd.Notification.Api"
RUN dotnet build "ShopFComputerBackEnd.Notification.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShopFComputerBackEnd.Notification.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopFComputerBackEnd.Notification.Api.dll"]