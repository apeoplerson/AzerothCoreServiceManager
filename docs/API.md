





# AzerothCore Manager API Documentation

## Overview
The AzerothCore Manager provides a REST API for managing Authserver and Worldserver processes on Windows.

## Base URL
```
http://localhost:8085
```

## Authentication
All endpoints except `/health` require Bearer token authentication.
Default token: `changeme123` (can be changed in `appsettings.json`)

## Endpoints

### Health Check
```http
GET /health
```
**Description**: Basic health check endpoint.

**Response**:
- `200 OK`: Service is running

### Status
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

### Auth Server Control

#### Start Auth Server
```http
POST /auth/start
```
**Description**: Start the Authserver process.

**Response**:
- `200 OK`: Success
- `500 Internal Server Error`: Failed to start server

#### Stop Auth Server
```http
POST /auth/stop
```
**Description**: Stop the Authserver process gracefully.

**Response**:
- `200 OK`: Success
- `500 Internal Server Error`: Failed to stop server

### World Server Control

#### Start World Server
```http
POST /world/start
```
**Description**: Start the Worldserver process.

**Response**:
- `200 OK`: Success
- `500 Internal Server Error`: Failed to start server

#### Stop World Server
```http
POST /world/stop
```
**Description**: Stop the Worldserver process gracefully.

**Response**:
- `200 OK`: Success
- `500 Internal Server Error`: Failed to stop server



