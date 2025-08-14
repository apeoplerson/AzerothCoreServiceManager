## Final Summary

### Project Structure

- src/
  - AzerothCoreManager.Service/
    - AuthMiddleware.cs
    - Config.cs
    - IProcessManager.cs
    - ProcessManager.cs
    - Program.cs
    - Worker.cs
    - appsettings.json

- packaging/
  - windows/
    - AzerothCoreManager.zip
    - README-WINDOWS.md
    - install.ps1
    - uninstall.ps1

- web/
  - ui/
    - src/
      - App.tsx
      - main.tsx

### Key Features

- REST API for programmatic control of Authserver and Worldserver
- Web interface for easy management
- Windows service for automatic startup and background operation
- Secure authentication using API tokens

### Installation Process

- Extract contents to a directory
- Run install script as Administrator
- Configure appsettings.json for your AzerothCore paths

### Usage

- Access web interface at http://localhost:5000
- Use REST API for programmatic control

### Uninstallation Process

- Run uninstall script as Administrator
- Delete installation directory
