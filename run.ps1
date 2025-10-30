param(
    [string]$Urls = "http://localhost:8080"
)

# Script to run the Web project from repository root with hot-reload
Set-Location -Path $PSScriptRoot
Write-Host "Starting Web project with Hot Reload on $Urls"
Set-Location -Path "$PSScriptRoot\Sistema.Universitario.Web"
dotnet watch run --urls $Urls
# Starts the web app with hot-reload from the workspace root without needing to pass --project
# Usage: .\run.ps1
$proj = "./Sistema.Universitario.Web/Sistema.Universitario.Web.csproj"
if (!(Test-Path $proj)) {
    Write-Error "Web project not found at $proj"
    exit 1
}
Write-Host "Starting dotnet watch for web project (http://localhost:8080)..."
dotnet watch --project $proj run --urls "http://localhost:8080"