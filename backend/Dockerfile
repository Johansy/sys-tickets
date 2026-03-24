FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY SysTickets.slnx .
COPY SysTickets.API/SysTickets.API.csproj SysTickets.API/
COPY SysTickets.Core/SysTickets.Core.csproj SysTickets.Core/
COPY SysTickets.Services/SysTickets.Services.csproj SysTickets.Services/
COPY SysTickets.Data/SysTickets.Data.csproj SysTickets.Data/
RUN dotnet restore
COPY . .
RUN dotnet publish SysTickets.API/SysTickets.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SysTickets.API.dll"]
