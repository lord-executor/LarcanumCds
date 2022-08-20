FROM executry/larcanum-cds-server:latest
WORKDIR /app

COPY ./dist/wiki-frontend ./wwwroot/view

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "LarcanumCds.Server.dll"]
