# 🚢 WifiAPIExam 📡

A .NET API for tracking and managing WiFi data usage across ships. This system provides endpoints for retrieving WiFi usage data, sales information, and ship IDs with authentication via Clerk.

## 📋 Table of Contents

- [✅ Requirements](#requirements)
- [🛠️ Setup](#setup)
  - [🐳 Docker Compose Setup (Recommended)](#docker-compose-setup-recommended)
  - [💾 Manual Database Setup](#manual-database-setup)
  - [⚙️ Configuration](#configuration)
- [▶️ Running the Application](#running-the-application)
- [🔌 API Endpoints](#api-endpoints)
- [🔐 Authentication](#authentication)
- [📂 Project Structure](#project-structure)
- [💻 Development](#development)

## ✅ Requirements

- 🟣 .NET 9.0 SDK
- 🐳 Docker (for PostgreSQL database)
- 🐘 PostgreSQL 16
- 🧰 Entity Framework Core tools

## 🛠️ Setup

### 🐳 Docker Compose Setup (Recommended)

The easiest way to get started is using Docker Compose, which automatically sets up both the database and the application:

```bash
docker compose up -d
```

This will:
- 📊 Start a PostgreSQL database container
- 🚀 Build and start the WifiAPIExam application
- 🔄 Configure all connection strings and environment variables
- ⚡ Make the API available at http://localhost:8080

No additional setup is required with this method!

### 💾 Manual Database Setup

If you prefer to set up components individually:

1. Start the PostgreSQL database using Docker:

```bash
docker run --name wifidb -e POSTGRES_USER=mats -e POSTGRES_PASSWORD=mats -e POSTGRES_DB=wifidb -p 5432:5432 -d postgres:16
```

2. Verify the database connection (optional):

```bash
docker exec -it wifidb psql -U mats -d wifidb
```

3. Navigate to the project directory:

```bash
cd WifiAPIExam
```

4. Run the Entity Framework migrations to set up the database schema:

```bash
dotnet ef migrations add InitialCreate # Only if migrations don't exist
dotnet ef database update
```

### ⚙️ Configuration

The application is configured via `appsettings.json`. Key settings include:

- **🗄️ Database Connection**: Configure your PostgreSQL connection string
- **🔑 Clerk Authentication**: Set your Clerk API secret key and allowed frontend origin

Example configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=wifidb;Username=mats;Password=mats"
  },
  "Clerk": {
    "SecretKey": "your_clerk_secret_key",
    "AllowedOrigin": "http://localhost:3000"
  }
}
```

## ▶️ Running the Application

1. Build and run the application:

```bash
dotnet run
```

2. The API will be available at:
   - 🌐 HTTP: http://localhost:5000
   - 🔒 HTTPS: https://localhost:5001
   - 📝 Swagger UI: https://localhost:5001/swagger

3. For Docker deployment, build the container:

```bash
docker build -t wifiapi .
docker run -p 8080:80 wifiapi
```

## 🔌 API Endpoints

The application provides several endpoints for retrieving WiFi data:

- **📊 GET /Wifi/DataUsage** - Get summarized WiFi data usage by period
- **💰 GET /Wifi/Sales** - Get sales information
- **🛳️ GET /Wifi/ShipIds** - Get available ship IDs

All endpoints require authentication using Clerk.

## 🔐 Authentication

This application uses Clerk for authentication. API requests must include a valid authorization token in the request header.

## 📂 Project Structure

- **🎮 Controllers/**: API endpoints
  - `WifiDataUsageController.cs`: Data usage endpoints
  - `WifiSalesController.cs`: Sales information endpoints
  - `WifiShipIdsController.cs`: Ship IDs endpoints
- **🗄️ Database/**: Database contexts and entity models
  - `WifiDbContext.cs`: EF Core database context
  - `Entities/`: Database entity models
- **⚙️ Services/**: Business logic services
  - `ImportService.cs`: Service for importing WiFi data
  - `AuthService.cs`: Authentication service using Clerk
  - `RolesService.cs`: Authorization service for role-based access
- **📝 Models/**: Data transfer objects
- **⚙️ Configuration/**: Application configuration classes

## 💻 Development

### 📥 Data Import

The application automatically imports WiFi usage data from the `wifi-usage-2025-04` directory on startup if the database is empty.

> ⚠️ **Important**: The `wifi-usage-2025-04` directory containing sample data files is not included in the GitHub repository. You will need to create this directory manually and add the appropriate JSON data files before running the application for the first time, unless you have existing data in the database.

Format of the WiFi data files should be JSON files named in the format `2025-04_{shipId}.json` placed in the `wifi-usage-2025-04` directory.

### 🧰 Useful Commands

- Clear all data from the database:
```sql
TRUNCATE TABLE "WifiDatabase" RESTART IDENTITY;
```

- View all WiFi data:
```sql
SELECT * FROM "WifiDatabase";
```

### 📚 Dependencies

Main project dependencies:
- 🔑 Clerk.BackendAPI (0.7.2)
- 🔌 Microsoft.AspNetCore.OpenApi (9.0.2)
- 🗄️ Microsoft.EntityFrameworkCore.Design (9.0.5)
- 🐘 Npgsql.EntityFrameworkCore.PostgreSQL (9.0.4)
- 📝 Swashbuckle.AspNetCore (8.1.1)
