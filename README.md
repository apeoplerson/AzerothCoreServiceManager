## Project Summary

### Overview

AzerothCore Manager is a Windows service that provides management capabilities for Authserver and Worldserver processes. It includes a REST API and web interface for controlling the servers.

### Features

- REST API for programmatic control of Authserver and Worldserver
- Web interface for easy management
- Windows service for automatic startup and background operation
- Secure authentication using API tokens

### Installation

1. Download the AzerothCore Manager zip file from the releases page.
2. Extract the contents to a directory (e.g., C:\Program Files\AzerothCoreManager).
3. Run the install script as Administrator:

   

### Configuration

Edit the appsettings.json file to match your AzerothCore paths:


http://localhost:5000

### REST API

- GET /health - Check service status
- GET /status - Get server status (auth and world)
- POST /auth/start - Start Authserver
- POST /auth/stop - Stop Authserver
- POST /world/start - Start Worldserver
- POST /world/stop - Stop Worldserver

All API endpoints require authentication using a Bearer token in the Authorization header.

### Uninstallation

To uninstall the service, run the following PowerShell commands as Administrator:

   
