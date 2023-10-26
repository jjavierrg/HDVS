#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
RUN bash -E $(curl -fsSL https://deb.nodesource.com/setup_16.x | bash - ); apt install -y nodejs
RUN apt-get install -y nodejs

WORKDIR /src
COPY ["EAPN.HDVS.Web/EAPN.HDVS.Web.csproj", "EAPN.HDVS.Web/"]
COPY ["EAPN.HDVS.Application/EAPN.HDVS.Application.csproj", "EAPN.HDVS.Application/"]
COPY ["EAPN.HDVS.Application.Core/EAPN.HDVS.Application.Core.csproj", "EAPN.HDVS.Application.Core/"]
COPY ["EAPN.HDVS.Infrastructure.Core/EAPN.HDVS.Infrastructure.Core.csproj", "EAPN.HDVS.Infrastructure.Core/"]
COPY ["EAPN.HDVS.Infrastructure/EAPN.HDVS.Infrastructure.csproj", "EAPN.HDVS.Infrastructure/"]
COPY ["EAPN.HDVS.Entities/EAPN.HDVS.Entities.csproj", "EAPN.HDVS.Entities/"]
COPY ["EAPN.HDVS.Shared/EAPN.HDVS.Shared.csproj", "EAPN.HDVS.Shared/"]

RUN dotnet restore "EAPN.HDVS.Web/EAPN.HDVS.Web.csproj"
COPY . .
WORKDIR "/src/EAPN.HDVS.Web"
RUN dotnet build "EAPN.HDVS.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EAPN.HDVS.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EAPN.HDVS.Web.dll"]