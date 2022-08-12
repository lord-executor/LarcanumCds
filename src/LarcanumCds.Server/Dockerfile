FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LarcanumCds.Server.csproj", "./"]
RUN dotnet restore "LarcanumCds.Server.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "LarcanumCds.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LarcanumCds.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LarcanumCds.Server.dll"]
