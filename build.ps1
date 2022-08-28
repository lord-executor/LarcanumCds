$ErrorActionPreference = "Inquire"

$srcPath = (Join-Path $PSScriptRoot "src")

Push-Location (Join-Path $srcPath "LarcanumCds.Server")
docker build -t executry/larcanum-cds-server .
Pop-Location

Push-Location (Join-Path $srcPath "Frontend/WikiFrontend")
ng build
docker build -t executry/larcanum-cds-wiki .
Pop-Location
