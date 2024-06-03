FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
RUN mkdir files
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/BFF.Web.API/BFF.Web.API.csproj", "BFF.Web.API/"]
COPY ["src/Infra/BFF.Web.Infra.CrossCutting.IoC/BFF.Web.Infra.CrossCutting.IoC.csproj", "Infra/BFF.Web.Infra.CrossCutting.IoC/"]
COPY ["src/BFF.Web.Services/BFF.Web.Services.csproj", "BFF.Web.Services/"]
COPY ["src/BFF.Web.Domain/BFF.Web.Domain.csproj", "BFF.Web.Domain/"]
RUN dotnet restore "BFF.Web.API/BFF.Web.API.csproj"
COPY src .
WORKDIR "/src/BFF.Web.API"
RUN dotnet build "BFF.Web.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BFF.Web.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BFF.Web.API.dll"]
