#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["1 - Application/Desafio.WebAPI/Desafio.WebAPI.csproj", "1 - Application/Desafio.WebAPI/"]
COPY ["2 - Domain/Desafio.Domain/Desafio.Domain.csproj", "2 - Domain/Desafio.Domain/"]
COPY ["4 - Infra/4.2 - CrossCutting/Desafio.Infra.CrossCutting/Desafio.Infra.CrossCutting.csproj", "4 - Infra/4.2 - CrossCutting/Desafio.Infra.CrossCutting/"]
COPY ["3 - Service/Desafio.Service/Desafio.Service.csproj", "3 - Service/Desafio.Service/"]
COPY ["4 - Infra/4.1 - Data/Desafio.Infra.Data/Desafio.Infra.Data.csproj", "4 - Infra/4.1 - Data/Desafio.Infra.Data/"]
RUN dotnet restore "1 - Application/Desafio.WebAPI/Desafio.WebAPI.csproj"
COPY . .
WORKDIR "/src/1 - Application/Desafio.WebAPI"
RUN dotnet build "/src/1 - Application/Desafio.WebAPI/Desafio.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "/src/1 - Application/Desafio.WebAPI/Desafio.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Desafio.WebAPI.dll"]