


param (
    [string]$Version = "1.0.0"
)

# (Optionally) ensure publish exists before MSI:
if (-not (Test-Path "..\..\dist\win-x64\AzerothCoreManager\AzerothCoreManager.Service.exe")) {
    Write-Host "Publish output missing. Run dotnet publish first or call the publish step here."
    exit 1
}

# WiX v4 build:
dotnet build "..\..\installer\wix\AzerothCoreManager.wixproj" -c Release `
    -p:Version=$Version `
    -p:ProductVersion=$Version `
    -p:Platform=x64

# Copy MSI to dist\msi
New-Item -Force -ItemType Directory "..\..\dist\msi" | Out-Null
Get-ChildItem "..\..\installer\wix\bin\Release\*.msi" | Copy-Item -Destination "..\..\dist\msi" -Force

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

