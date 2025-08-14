

# AzerothCore Manager

AzerothCore Manager is a Windows service that provides management capabilities for Authserver and Worldserver processes. It includes a REST API and web interface for controlling the servers.

![AzerothCore Manager Dashboard](https://github.com/apeoplerson/AzerothCoreServiceManager2/raw/main/web/ui/public/vite.svg)

## Table of Contents

- [Features](#features)
- [Installation](#installation)
  - [Prerequisites](#prerequisites)
  - [Using MSI Installer (Recommended)](#using-msi-installer-recommended)
  - [Manual Installation](#manual-installation)
- [Configuration](#configuration)
- [Usage](#usage)
  - [Web Interface](#web-interface)
  - [REST API](#rest-api)
  - [Command Line](#command-line)
- [Uninstallation](#uninstallation)
- [Troubleshooting](#troubleshooting)
- [Development](#development)

## Features

- **Windows Service**: Manages Authserver and Worldserver processes with start/stop/restart capabilities
- **REST API**: Secure local API for managing servers and viewing logs
- **Web Interface**: React-based interface for easy management
- **Process Control**: Graceful shutdown, crash detection, auto-restart on crash
- **Log Management**: Log tailing, search, and structured JSON logging
- **Config Editor**: Edit AzerothCore configuration files with backup/rollback
- **Security**: Bearer token authentication, IP allowlist support

## Installation

### Prerequisites

Ensure you have:
- Windows 10 or later
- .NET 8 runtime installed (can be downloaded from [Microsoft](https://dotnet.microsoft.com/download/dotnet/8.0))
- AzerothCore installed and configured on your system

### Using MSI Installer (Recommended)

1. **Download the latest MSI installer** from the [releases page](https://github.com/apeoplerson/AzerothCoreServiceManager2/releases)
2. **Run the installer** as Administrator
3. **Follow the prompts** to complete installation
4. **Access the web interface** at `http://localhost:8085`

### Manual Installation

1. **Download** the AzerothCoreManager zip file from the [releases page](https://github.com/apeoplerson/AzerothCoreServiceManager2/releases)
2. **Extract** to a directory (e.g., `C:\Program Files\AzerothCoreManager`)
3. **Configure** `appsettings.json` with your AzerothCore paths
4. **Install the service**:
   ```powershell
   cd C:\Program Files\AzerothCoreManager
   .\install.ps1
   ```
5. **Access** the web interface at `http://localhost:8085`

## Configuration

The main configuration file is `appsettings.json`. Here's an example configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AzerothCorePaths": {
    "AuthServerPath": "C:\\azerothcore\\bin\\authserver.exe",
    "WorldServerPath": "C:\\azerothcore\\bin\\worldserver.exe",
    "LogsDirectory": "C:\\azerothcore\\logs",
    "ConfigsDirectory": "C:\\azerothcore\\etc"
  },
  "Security": {
    "ApiToken": "changeme123",      // Bearer token for API authentication
    "AutoRestartOnCrash": true     // Auto-restart servers if they crash
  }
}
```

You can override the API token using environment variables:
- `ACM_TOKEN`: Override the API token from appsettings.json

## Usage

### Web Interface

The web interface provides a user-friendly way to manage your servers:

#### Dashboard
- View real-time status of Authserver and Worldserver
- Start, stop, or restart both servers
- Monitor CPU/RAM usage

#### Logs
- View live log output from both servers
- Search through logs
- Tail the most recent log entries

#### Config Editor
- Edit AzerothCore configuration files (.conf)
- View diffs between versions
- Backup and restore previous configurations

#### Settings
- Manage API token (rotate for security)
- Configure auto-restart behavior
- Set paths to server executables

### REST API

The AzerothCore Manager provides a REST API for managing Authserver and Worldserver processes on Windows.

**Base URL**: `http://localhost:8085`

**Authentication**: All endpoints except `/health` require Bearer token authentication. Default token: `changeme123` (can be changed in `appsettings.json`)

#### Endpoints

##### Health Check
```http
GET /health
```
**Description**: Basic health check endpoint.

**Response**:
- `200 OK`: Service is running

##### Status
```http
GET /status
```
**Description**: Get current status of Authserver and Worldserver.

**Response**:
```json
{
  "auth": {
    "running": true,
    "pid": 12345
  },
  "world": {
    "running": false,
    "pid": 0
  }
}
```

##### Auth Server Control

###### Start Auth Server
```http
POST /auth/start
```
**Description**: Start the Authserver process.

**Response**:
- `200 OK`: Success
- `500 Internal Server Error`: Failed to start server

###### Stop Auth Server
```http
POST /auth/stop
```
**Description**: Stop the Authserver process gracefully.

**Response**:
- `200 OK`: Success
- `500 Internal Server Error`: Failed to stop server

##### World Server Control

###### Start World Server
```http
POST /world/start
```
**Description**: Start the Worldserver process.

**Response**:
- `200 OK`: Success
- `500 Internal Server Error`: Failed to start server

###### Stop World Server
```http
POST /world/stop
```
**Description**: Stop the Worldserver process gracefully.

**Response**:
- `200 OK`: Success
- `500 Internal Server Error`: Failed to stop server

### Command Line

You can also use the REST API directly:

```bash
# Get status
curl http://localhost:8085/status -H "Authorization: Bearer changeme123"

# Start Authserver
curl -X POST http://localhost:8085/auth/start -H "Authorization: Bearer changeme123"

# Stop Worldserver
curl -X POST http://localhost:8085/world/stop -H "Authorization: Bearer changeme123"
```

## Uninstallation

To remove the service:

```powershell
cd C:\Program Files\AzerothCoreManager
.\uninstall.ps1
```

This will stop and remove the Windows service, and clean up firewall rules.

For MSI installations:

```powershell
# Silent uninstall:
msiexec /x .\dist\msi\AzerothCoreManager_1.0.0.msi /qn
```

## Troubleshooting

- Check logs in `C:\Program Files\AzerothCoreManager\logs` for errors.
- Ensure your AzerothCore paths are correctly configured.
- Verify that the API token matches between client requests and server configuration.
- If the service doesn't start, check Windows Event Viewer for error messages.

## Development

### Building from Source

1. **Clone the repository**:
   ```bash
   git clone https://github.com/apeoplerson/AzerothCoreServiceManager2.git
   cd AzerothCoreServiceManager2
   ```

2. **Build the .NET service**:
   ```bash
   cd src/AzerothCoreManager.Service
   dotnet build -c Release
   ```

3. **Build the React UI**:
   ```bash
   cd ../../web/ui
   npm install
   npm run build
   ```

4. **Cross-compile for Windows**:
   ```bash
   cd ../..
   dotnet publish ./src/AzerothCoreManager.Service -c Release -r win-x64 --self-contained true -p:PublishTrimmed=true -o ./dist/win-x64/AzerothCoreManager
   ```

### Building the MSI Installer

```powershell
# From repo root:
powershell -ExecutionPolicy Bypass -File installer\scripts\build-msi.ps1 -Version 1.0.0
```

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgments

- [AzerothCore](https://github.com/azerothcore/azerothcore-wotlk) - The World of Warcraft emulator this manager controls
- [WiX Toolset](https://wixtoolset.org/) - Used for MSI packaging
- [React](https://reactjs.org/) - Web interface framework
- [Serilog](https://serilog.net/) - Structured logging

## Support

For support, please open an issue on the [GitHub repository](https://github.com/apeoplerson/AzerothCoreServiceManager2/issues).

---

**AzerothCore Manager** - Making AzerothCore server management easy and efficient!

