# AzerothCore Manager Installer Documentation

## Windows Prerequisites
- .NET 8 SDK
- WiX v4 dotnet tool (installed automatically by build script)

## Building the MSI
From repo root:
```powershell
powershell -ExecutionPolicy Bypass -File installer\scripts\build-msi.ps1 -Version 1.0.0
```
This will output the MSI to: `./dist/msi/AzerothCoreManager_1.0.0.msi`

## Silent Install/Uninstall
```powershell
# Silent install
msiexec /i .\dist\msi\AzerothCoreManager_1.0.0.msi /qn

# Silent uninstall
msiexec /x .\dist\msi\AzerothCoreManager_1.0.0.msi /qn
```

## Upgrade Behavior
- Existing `appsettings.json` is preserved on upgrade (not overwritten)
- Logs directory contents are preserved
- Service settings and firewall rules are updated

## File Preservation Notes
- User modifications to `appsettings.json` are kept during upgrades
- Log files in the logs directory are not removed during uninstall
