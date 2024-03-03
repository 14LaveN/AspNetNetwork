FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AspNetNetwork.Micro.MessagingAPI/AspNetNetwork.Micro.MessagingAPI.csproj", "AspNetNetwork.Micro.MessagingAPI/"]
COPY ["AspNetNetwork.Application/AspNetNetwork.Application.csproj", "AspNetNetwork.Application/"]
COPY ["AspNetNetwork.Email/AspNetNetwork.Email.csproj", "AspNetNetwork.Email/"]
COPY ["AspNetNetwork.Database.Identity/AspNetNetwork.Database.Identity.csproj", "AspNetNetwork.Database.Identity/"]
COPY ["AspNetNetwork.Domain.Common/AspNetNetwork.Domain.Common.csproj", "AspNetNetwork.Domain.Common/"]
COPY ["AspNetNetwork.Domain.Identity/AspNetNetwork.Domain.Identity.csproj", "AspNetNetwork.Domain.Identity/"]
COPY ["AspNetNetwork.Cache/AspNetNetwork.Cache.csproj", "AspNetNetwork.Cache/"]
COPY ["AspNetNetwork.Database.MetricsAndMessages/AspNetNetwork.Database.MetricsAndMessages.csproj", "AspNetNetwork.Database.MetricsAndMessages/"]
COPY ["AspNetNetwork.Database.Message/AspNetNetwork.Database.Message.csproj", "AspNetNetwork.Database.Message/"]
COPY ["AspNetNetwork.Database.Common/AspNetNetwork.Database.Common.csproj", "AspNetNetwork.Database.Common/"]
COPY ["AspNetNetwork.Domain.Message/AspNetNetwork.Domain.Message.csproj", "AspNetNetwork.Domain.Message/"]
RUN dotnet restore "AspNetNetwork.Micro.MessagingAPI/AspNetNetwork.Micro.MessagingAPI.csproj"
COPY . .
WORKDIR "/src/AspNetNetwork.Micro.MessagingAPI"
RUN dotnet build "AspNetNetwork.Micro.MessagingAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspNetNetwork.Micro.MessagingAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspNetNetwork.Micro.MessagingAPI.dll"]
