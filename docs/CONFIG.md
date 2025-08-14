






# AzerothCore Manager Configuration

## appsettings.json
The main configuration file for the AzerothCore Manager service.

### Logging
```json
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore": "Warning"
  }
}
```

### AzerothCore Paths
Configure paths to your AzerothCore installation:
```json
"AzerothCorePaths": {
  "AuthServerPath": "C:\\azerothcore\\bin\\authserver.exe",
  "WorldServerPath": "C:\\azerothcore\\bin\\worldserver.exe",
  "LogsDirectory": "C:\\azerothcore\\logs",
  "ConfigsDirectory": "C:\\azerothcore\\etc"
}
```

### Security
Security settings including API token and auto-restart behavior:
```json
"Security": {
  "ApiToken": "changeme123",      // Bearer token for API authentication
  "AutoRestartOnCrash": true     // Auto-restart servers if they crash
}
```

## Environment Variables
You can override the API token using environment variables:
- `ACM_TOKEN`: Override the API token from appsettings.json




