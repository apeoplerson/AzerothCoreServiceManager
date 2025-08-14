





<#
.SYNOPSIS
Install AzerothCore Manager Windows Service

.DESCRIPTION
This script installs the AzerothCore Manager service on Windows, configures it to start automatically,
and sets up firewall rules for local access.

.PARAMETER Path
Path to the AzerothCoreManager executable directory (default: current directory)

.EXAMPLE
.\install.ps1 -Path "C:\Program Files\AzerothCoreManager"
#>

param(
    [string]$Path = (Get-Location).Path
)

Write-Host "AzerothCore Manager Installer"
Write-Host "============================="

# Stop existing service if present
$serviceName = "AzerothCoreManager"
if (Get-Service -Name $serviceName -ErrorAction SilentlyContinue) {
    Write-Host "Stopping existing AzerothCoreManager service..."
    Stop-Service -Name $serviceName -Force -ErrorAction SilentlyContinue | Out-Null
}

# Copy files to installation directory
$installDir = "$Path"
if (-not (Test-Path -Path $installDir)) {
    New-Item -ItemType Directory -Path $installDir | Out-Null
}
Write-Host "Copying files to $installDir..."

# Create logs and configs directories if they don't exist
$logsDir = "$installDir\logs"
$configsDir = "$installDir\etc"
if (-not (Test-Path -Path $logsDir)) {
    New-Item -ItemType Directory -Path $logsDir | Out-Null
}
if (-not (Test-Path -Path $configsDir)) {
    New-Item -ItemType Directory -Path $configsDir | Out-Null
}

# Copy service executable and dependencies
Copy-Item -Path "AzerothCoreManager.Service.exe" -Destination "$installDir\" -Force
Copy-Item -Path "appsettings.json" -Destination "$installDir\" -Force

Write-Host "Creating AzerothCore Manager service..."

# Create the Windows service
$serviceExe = "$installDir\AzerothCoreManager.Service.exe"
New-Service -Name $serviceName -BinaryPathName $serviceExe -StartupType Automatic

Write-Host "Starting AzerothCore Manager service..."
Start-Service -Name $serviceName

Write-Host "Configuring firewall rules for local access..."

# Configure firewall to allow local connections only
netsh advfirewall firewall add rule name="AzerothCoreManager Local" dir=in action=allow protocol=TCP localport=8085 remoteip=127.0.0.1

Write-Host "Installation complete!"
Write-Host "Service is running and accessible at http://localhost:8085"
Write-Host "Default API token: changeme123 (change in appsettings.json)"


