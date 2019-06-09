# COMMON PATHS

$buildFolder = (Get-Item -Path "./" -Verbose).FullName
$slnFolder = Join-Path $buildFolder "../"
$outputFolder = Join-Path $buildFolder "hostoutputs"
$webHostFolder = Join-Path $slnFolder "src/Monivault.Web.Host"

## CLEAR ######################################################################

Remove-Item $outputFolder -Force -Recurse -ErrorAction Ignore
New-Item -Path $outputFolder -ItemType Directory

## RESTORE NUGET PACKAGES #####################################################

Set-Location $slnFolder
dotnet restore

## PUBLISH WEB MVC PROJECT ###################################################

Set-Location $webHostFolder
dotnet publish --output (Join-Path $outputFolder "Host")

## CREATE DOCKER IMAGES #######################################################

# Mvc
Set-Location (Join-Path $outputFolder "Host")

docker rmi monivault-api -f
docker build -t monivault-api .

## DOCKER COMPOSE FILES #######################################################

Copy-Item (Join-Path $slnFolder "docker/host/*.*") $outputFolder

## FINALIZE ###################################################################

Set-Location $outputFolder