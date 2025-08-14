








# AzerothCore Manager - Windows Installation Guide

## Overview
AzerothCore Manager is a Windows service that provides management capabilities for Authserver and Worldserver processes. It includes a REST API and web interface for controlling the servers.

## Prerequisites
- Windows 10 or later
- .NET 8 runtime (included with the self-contained build)
- AzerothCore installation on your system

## Installation

### Method 1: Using PowerShell Scripts (Recommended)

1. **Extract** the AzerothCoreManager zip file to a directory (e.g., `C:\Program Files\AzerothCoreManager`).
2. **Open PowerShell as Administrator**.
3. **Navigate** to the extraction directory.
4. **Run** the install script:
   ```powershell
   .\install.ps1
   ```

### Method 2: Manual Installation

1. **Extract** the AzerothCoreManager zip file to a directory (e.g., `C:\Program Files\AzerothCoreManager`).
2. **Create logs and configs directories** if they don't exist:
   - `C:\Program Files\AzerothCoreManager\logs`
   - `C:\Program Files\AzerothCoreManager\etc`
3. **Create a Windows service**:
   ```powershell
   New-Service -Name "AzerothCoreManager" -BinaryPathName "C:\Program Files\AzerothCoreManager\AzerothCoreManager.Service.exe" -StartupType Automatic
   ```
4. **Start the service**:
   ```powershell
   Start-Service AzerothCoreManager
   ```
5. **Configure firewall** to allow HTTP/HTTPS traffic if needed.

## Configuration

Edit the `appsettings.json` file in the installation directory to match your AzerothCore paths:

```json
{
  "AzerothCorePaths": {
    "AuthServerPath": "C:\\path\\to\\your\\authserver.exe",
    "WorldServerPath": "C:\\path\\to\\your\\worldserver.exe",
    "LogsDirectory": "C:\\path\\to\\logs",
    "ConfigsDirectory": "C:\\path\\to\\configs"
  },
  "Security": {
    "ApiToken": "your-secure-token-here",
    "AutoRestartOnCrash": true
  }
}
```

## Usage

### Web Interface
After installation, open your web browser and navigate to:
```
http://localhost:5000
```

### REST API
The service provides a REST API for programmatic control:

- **GET** `/health` - Check service status
- **GET** `/status` - Get server status (auth and world)
- **POST** `/auth/start` - Start Authserver
- **POST** `/auth/stop` - Stop Authserver
- **POST** `/world/start` - Start Worldserver
- **POST** `/world/stop` - Stop Worldserver

All API endpoints require authentication using a Bearer token in the `Authorization` header.

## Uninstallation

To uninstall the service, run the following PowerShell commands as Administrator:

```powershell
Stop-Service AzerothCoreManager
Remove-Service AzerothCoreManager
```

Then delete the installation directory.

## Troubleshooting

### Service won't start
1. Check the logs in `C:\Program Files\AzerothCoreManager\logs\manager.log`
2. Ensure all paths in `appsettings.json` are correct and accessible
3. Verify that AzerothCore executables exist at the specified locations

### Web interface not accessible
1. Make sure no other service is using port 5000
2. Check firewall settings to ensure HTTP/HTTPS traffic is allowed
3. Try accessing the service from a different machine if needed

## License

This software is licensed under the MIT License. See the LICENSE file for details.





