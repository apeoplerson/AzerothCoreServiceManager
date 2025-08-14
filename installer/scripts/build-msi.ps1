param (
    [string]$Version = "1.0.0"
)

# Set version
$env:Version = $Version

# Install WiX tool if needed
if (-not (Get-Command wix -ErrorAction SilentlyContinue)) {
    dotnet tool install --global wix --version 4.*
}

# Harvest files from dist directory
wix heat directory ..\..\dist\win-x64\AzerothCoreManager -gg -srd -cg AppComponents -dr INSTALLDIR -out Files.wxs

# Build the MSI
dotnet build ..\AzerothCoreManager.wixproj /p:Version=$Version

# Output path
$msiPath = "..\..\dist\msi\AzerothCoreManager_$Version.msi"

if (Test-Path $msiPath) {
    Write-Host "MSI created successfully at: $msiPath"
    Write-Host "Silent install command:"
    Write-Host "msiexec /i `$msiPath /qn"
    Write-Host "Silent uninstall command:"
    Write-Host "msiexec /x `$msiPath /qn"
} else {
    Write-Error "MSI build failed!"
    exit 1
}
