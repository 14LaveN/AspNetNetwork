FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AspNetNetwork.Micro.IdentityAPI/AspNetNetwork.Micro.IdentityAPI.csproj", "AspNetNetwork.Micro.IdentityAPI/"]
COPY ["AspNetNetwork.Domain.Identity/AspNetNetwork.Domain.Identity.csproj", "AspNetNetwork.Domain.Identity/"]
COPY ["AspNetNetwork.Domain.Common/AspNetNetwork.Domain.Common.csproj", "AspNetNetwork.Domain.Common/"]
COPY ["AspNetNetwork.Database.Identity/AspNetNetwork.Database.Identity.csproj", "AspNetNetwork.Database.Identity/"]
COPY ["AspNetNetwork.Database.Attendee/AspNetNetwork.Database.Attendee.csproj", "AspNetNetwork.Database.Attendee/"]
COPY ["AspNetNetwork.Database.Common/AspNetNetwork.Database.Common.csproj", "AspNetNetwork.Database.Common/"]
COPY ["AspNetNetwork.Database.PersonalEvent/AspNetNetwork.Database.PersonalEvent.csproj", "AspNetNetwork.Database.PersonalEvent/"]
COPY ["AspNetNetwork.Database.GroupEvent/AspNetNetwork.Database.GroupEvent.csproj", "AspNetNetwork.Database.GroupEvent/"]
COPY ["AspNetNetwork.Database.Invitation/AspNetNetwork.Database.Invitation.csproj", "AspNetNetwork.Database.Invitation/"]
COPY ["AspNetNetwork.Database.Notification/AspNetNetwork.Database.Notification.csproj", "AspNetNetwork.Database.Notification/"]
COPY ["AspNetNetwork.Application/AspNetNetwork.Application.csproj", "AspNetNetwork.Application/"]
COPY ["AspNetNetwork.Email/AspNetNetwork.Email.csproj", "AspNetNetwork.Email/"]
COPY ["AspNetNetwork.Cache/AspNetNetwork.Cache.csproj", "AspNetNetwork.Cache/"]
RUN dotnet restore "AspNetNetwork.Micro.IdentityAPI/AspNetNetwork.Micro.IdentityAPI.csproj"
COPY . .
WORKDIR "/src/AspNetNetwork.Micro.IdentityAPI"
RUN dotnet build "AspNetNetwork.Micro.IdentityAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspNetNetwork.Micro.IdentityAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspNetNetwork.Micro.IdentityAPI.dll"]
