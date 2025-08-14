







# AzerothCore Manager Operations Guide

## Installation
1. **Prerequisites**: Ensure you have .NET 8 runtime installed on Windows.
2. **Download**: Copy the AzerothCoreManager zip file to your Windows machine.
3. **Extract**: Extract the contents to a directory (e.g., `C:\Program Files\AzerothCoreManager`).
4. **Install Service**:
   ```powershell
   cd C:\Program Files\AzerothCoreManager
   .\install.ps1
   ```
5. **Access**: Open your browser and navigate to `http://localhost:8085`

## Configuration
- Edit `appsettings.json` before running the service for the first time.
- Configure paths to your AzerothCore installation.
- Set the API token (default: `changeme123`).

## Web UI Usage
The web interface provides a user-friendly way to manage your servers:

### Dashboard
- View real-time status of Authserver and Worldserver
- Start, stop, or restart both servers
- Monitor CPU/RAM usage

### Logs
- View live log output from both servers
- Search through logs
- Tail the most recent log entries

### Config Editor
- Edit AzerothCore configuration files (.conf)
- View diffs between versions
- Backup and restore previous configurations

### Settings
- Manage API token (rotate for security)
- Configure auto-restart behavior
- Set paths to server executables

## Command Line Usage
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

## Troubleshooting
- Check logs in `C:\Program Files\AzerothCoreManager\logs` for errors.
- Ensure your AzerothCore paths are correctly configured.
- Verify that the API token matches between client requests and server configuration.



