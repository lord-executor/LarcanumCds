# Overview
TODO

## Building

### LarcanumCds.Server Docker Image
From within the `src/LarcanumCds.Server` directory

```shell
docker build -t executry/larcanum-cds-server .
docker push executry/larcanum-cds-server:latest
```

### LarcanumCds Wiki Frontend Docker Image
From within the `src/Frontend/WikiFrontend` directory

```shell
ng build
docker build -t executry/larcanum-cds-wiki .
docker push executry/larcanum-cds-wiki:latest
```
