FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AspNetNetwork.Web/AspNetNetwork.Web.csproj", "AspNetNetwork.Web/"]
RUN dotnet restore "AspNetNetwork.Web/AspNetNetwork.Web.csproj"
COPY . .
WORKDIR "/src/AspNetNetwork.Web"
RUN dotnet build "AspNetNetwork.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspNetNetwork.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspNetNetwork.Web.dll"]
