FROM executry/larcanum-cds-server:latest
WORKDIR /app

COPY ./dist/wiki-frontend ./wwwroot

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "LarcanumCds.Server.dll"]
