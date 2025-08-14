# AzerothCore Manager - Final Summary

## Project Overview
AzerothCore Manager is a Windows service that provides management capabilities for Authserver and Worldserver processes. It includes a REST API and web interface for controlling the servers.

## Key Features
- REST API for programmatic control of Authserver and Worldserver
- Web interface for easy management
- Windows service for automatic startup and background operation
- Secure authentication using API tokens

## Installation Process
1. Extract contents to a directory
2. Run install script as Administrator
3. Configure appsettings.json for your AzerothCore paths

## Usage
- Access web interface at http://localhost:5000
- Use REST API for programmatic control

## Uninstallation Process
1. Run uninstall script as Administrator
2. Delete installation directory

## MSI Packaging (New)
We've added MSI packaging using WiX v4 with the following features:
- Installs to C:\Program Files\AzerothCoreManager
- Installs/updates the Windows Service and starts it
- Creates Start Menu shortcut to the local Web UI
- Preserves existing appsettings.json on upgrade
- Adds a Windows Firewall rule for localhost-only access
- Supports clean uninstall (stops service, removes files)

## Files Created
1. `/installer/wix/AzerothCoreManager.wxs` - Main WiX source file
2. `/installer/wix/AzerothCoreManager.wixproj` - WiX project file
3. `/installer/scripts/build-msi.ps1` - Build script for MSI creation
4. `/installer/scripts/sign.ps1` - Signing script for MSI
5. `/docs/INSTALLER.md` - Installer documentation

## Build and Sign the MSI
```powershell
# From repo root:
powershell -ExecutionPolicy Bypass -File installer\scripts\build-msi.ps1 -Version 1.0.0

# To sign (if you have a PFX certificate):
$env:SIGNING_PFX="path\to\cert.pfx"
$env:SIGNING_PFX_PASSWORD="password"
powershell -ExecutionPolicy Bypass -File installer\scripts\sign.ps1 `
    -msiPath .\dist\msi\AzerothCoreManager_1.0.0.msi `
    -pfxPath $env:SIGNING_PFX `
    -pfxPassword $env:SIGNING_PFX_PASSWORD
```

## Silent Install/Uninstall Commands
```powershell
# Silent install:
msiexec /i .\dist\msi\AzerothCoreManager_1.0.0.msi /qn

# Silent uninstall:
msiexec /x .\dist\msi\AzerothCoreManager_1.0.0.msi /qn
```

## Upgrade Behavior
- Existing appsettings.json is preserved (not overwritten)
- Logs directory contents are preserved
- Service settings and firewall rules are updated

## File Preservation Notes
- User modifications to appsettings.json are kept during upgrades
- Log files in the logs directory are not removed during uninstall
