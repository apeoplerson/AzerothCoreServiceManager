






<#
.SYNOPSIS
Uninstall AzerothCore Manager Windows Service

.DESCRIPTION
This script stops and removes the AzerothCore Manager service from Windows,
and cleans up firewall rules.

.PARAMETER Path
Path to the AzerothCoreManager executable directory (default: current directory)

.EXAMPLE
.\uninstall.ps1 -Path "C:\Program Files\AzerothCoreManager"
#>

param(
    [string]$Path = (Get-Location).Path
)

Write-Host "AzerothCore Manager Uninstaller"
Write-Host "=============================="

$serviceName = "AzerothCoreManager"

# Stop the service if it's running
if (Get-Service -Name $serviceName -ErrorAction SilentlyContinue) {
    Write-Host "Stopping AzerothCoreManager service..."
    Stop-Service -Name $serviceName -Force | Out-Null
}

# Remove the Windows service
Write-Host "Removing AzerothCore Manager service..."
Remove-Service -Name $serviceName

# Remove firewall rule
Write-Host "Removing firewall rules..."
netsh advfirewall firewall delete rule name="AzerothCoreManager Local"

Write-Host "Uninstallation complete!"

