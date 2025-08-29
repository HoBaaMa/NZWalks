# 🚶‍♂️ NZWalks API

A comprehensive ASP.NET Core Web API for managing walking trails and hiking routes in New Zealand. This API provides full CRUD operations for walks, regions, and user management with JWT authentication, image upload capabilities, advanced filtering, sorting, and pagination features.

## 📋 Table of Contents

- [✨ Features](#-features)
- [🚀 Technologies Used](#-technologies-used)
- [🏗️ Getting Started](#️-getting-started)
  - [📋 Prerequisites](#-prerequisites)
  - [⚙️ Setup](#️-setup)
- [🔗 API Endpoints](#-api-endpoints)
- [📄 Example JSON](#-example-json)
- [🎯 Models](#-models)
- [❌ Error Handling](#-error-handling)
- [📦 NuGet Packages](#-nuget-packages)
- [⚡ Quick Install](#-quick-install)
- [🛡️ Authentication & Security](#️-authentication--security)
- [📖 Project Architecture](#-project-architecture)
- [📊 Logging & Monitoring](#-logging--monitoring)
- [📜 License](#-license)

## ✨ Features

- **🚶‍♂️ Walk Management**: Full CRUD operations for walking trails
- **🌍 Region Management**: Manage geographical regions and locations
- **🔐 JWT Authentication**: Secure token-based authentication
- **👥 Role-Based Authorization**: Reader and Writer roles with appropriate permissions
- **📸 Image Upload**: Local file upload and management for trail images
- **🔍 Advanced Filtering**: Filter walks by name, region, and difficulty
- **📊 Sorting & Pagination**: Sort results and paginate large datasets
- **✅ Model Validation**: Comprehensive data validation with custom attributes
- **📝 Structured Logging**: Detailed logging with Serilog
- **🛡️ Global Exception Handling**: Centralized error handling middleware
- **📚 API Documentation**: Swagger/OpenAPI integration with JWT support
- **🏗️ Clean Architecture**: Repository pattern with dependency injection

## 🚀 Technologies Used

- **.NET 8** - Latest .NET framework
- **ASP.NET Core Web API** - Web API framework  
- **Entity Framework Core 9.0** - Object-relational mapper
- **SQL Server** - Database engine
- **JWT Bearer Authentication** - Token-based security
- **ASP.NET Core Identity** - User management system
- **AutoMapper 12.0** - Object-to-object mapping
- **Serilog** - Structured logging framework
- **Swagger/OpenAPI** - API documentation
- **File Upload Support** - Image handling capabilities

## 🏗️ Getting Started

### 📋 Prerequisites

- **.NET 8 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server** - Local or remote instance
- **Visual Studio 2022** or **Visual Studio Code** (recommended)
- **Git** - For version control

### ⚙️ Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/HoBaaMa/NZWalks-API.git
   cd NZWalks-API
   ```

2. **Update Connection Strings**
   
   Edit `appsettings.json` and update the connection strings:
   ```json
   {
     "ConnectionStrings": {
       "NZWalksConnectionString": "Server=YourServer;Database=NZWalksDb;Trusted_Connection=True;TrustServerCertificate=True",
       "NZWalksAuthConnectionString": "Server=YourServer;Database=NZwalksAuthDb;Trusted_Connection=True;TrustServerCertificate=True"
     }
   }
   ```

3. **Apply Database Migrations**
   ```bash
   cd "NZWalks API"
   dotnet ef database update --context NZWalksDbContext
   dotnet ef database update --context NZWalksAuthDbContext
   ```

4. **Build and Run the Application**
   ```bash
   dotnet build
   dotnet run
   ```

5. **Access the API**
   - **API Base URL**: `https://localhost:7247` or `http://localhost:5000`
   - **Swagger UI**: `https://localhost:7247/swagger`

## 🔗 API Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| **Authentication** |
| `POST` | `/api/Auth/Register` | Register a new user | ❌ |
| `POST` | `/api/Auth/Login` | Authenticate user and get JWT token | ❌ |
| **Regions** |
| `GET` | `/api/Regions` | Get all regions | ✅ (Reader/Writer) |
| `GET` | `/api/Regions/{id}` | Get region by ID | ✅ (Reader/Writer) |
| `POST` | `/api/Regions` | Create a new region | ✅ (Writer) |
| `PUT` | `/api/Regions/{id}` | Update a region | ✅ (Writer) |
| `DELETE` | `/api/Regions/{id}` | Delete a region | ✅ (Writer) |
| **Walks** |
| `GET` | `/api/Walks` | Get all walks with filtering & pagination | ✅ (Reader/Writer) |
| `GET` | `/api/Walks/{id}` | Get walk by ID | ✅ (Reader/Writer) |
| `POST` | `/api/Walks` | Create a new walk | ✅ (Writer) |
| `PUT` | `/api/Walks/{id}` | Update a walk | ✅ (Writer) |
| `DELETE` | `/api/Walks/{id}` | Delete a walk | ✅ (Writer) |
| **Images** |
| `POST` | `/api/Images/Upload` | Upload an image file | ✅ (Writer) |

### 🔍 Query Parameters for Walks

The `/api/Walks` endpoint supports the following query parameters:

- `filterOn` - Field to filter by (e.g., "Name")
- `filterQuery` - Filter value
- `sortBy` - Field to sort by (e.g., "Name", "LengthInKm")
- `isAscending` - Sort direction (true/false)
- `pageNumber` - Page number (default: 1)
- `pageSize` - Items per page (default: 10)

**Example**: `/api/Walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10`

## 📄 Example JSON

### 🔐 Authentication

**Register Request**:
```json
{
  "userName": "user@example.com",
  "password": "Password123",
  "roles": ["Reader", "Writer"]
}
```

**Login Request**:
```json
{
  "userName": "user@example.com", 
  "password": "Password123"
}
```

**Login Response**:
```json
{
  "jwtToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "message": "Login successful"
}
```

### 🌍 Regions

**Create Region Request**:
```json
{
  "code": "AKL",
  "name": "Auckland",
  "imageUrl": "https://example.com/auckland.jpg"
}
```

**Region Response**:
```json
{
  "id": "57da9f56-af31-4d60-b753-e90c01374d99",
  "code": "AKL", 
  "name": "Auckland",
  "imageUrl": "https://example.com/auckland.jpg"
}
```

### 🚶‍♂️ Walks

**Create Walk Request**:
```json
{
  "name": "Milford Track",
  "description": "A world-renowned walking track through pristine wilderness",
  "lengthInKm": 53.5,
  "imageUrl": "https://example.com/milford-track.jpg",
  "difficultyId": "e2975a33-fdb3-4636-b662-0481bc8cc67d",
  "regionId": "57da9f56-af31-4d60-b753-e90c01374d99"
}
```

**Walk Response**:
```json
{
  "name": "Milford Track",
  "description": "A world-renowned walking track through pristine wilderness", 
  "lengthInKm": 53.5,
  "imageUrl": "https://example.com/milford-track.jpg",
  "region": {
    "id": "57da9f56-af31-4d60-b753-e90c01374d99",
    "code": "STL",
    "name": "Southland",
    "imageUrl": null
  },
  "difficulty": {
    "id": "e2975a33-fdb3-4636-b662-0481bc8cc67d", 
    "name": "Hard"
  }
}
```

## 🎯 Models

### Walk
```csharp
public class Walk
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double LengthInKm { get; set; }
    public string? ImageUrl { get; set; }
    public Guid DifficultyId { get; set; }
    public Difficulty Difficulty { get; set; }
    public Guid RegionId { get; set; }
    public Region Region { get; set; }
}
```

### Region
```csharp
public class Region
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? ImageUrl { get; set; }
}
```

### Difficulty
```csharp
public class Difficulty
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
```

### Image
```csharp
public class Image
{
    public Guid Id { get; set; }
    [NotMapped]
    public IFormFile File { get; set; }
    public string FileName { get; set; }
    public string? FileDescription { get; set; }
    public string FileExtension { get; set; }
    public long FileSizeInBytes { get; set; }
    public string FilePath { get; set; }
}
```

## ❌ Error Handling

The API implements comprehensive error handling:

- **400 Bad Request** - Invalid input data or validation errors
- **401 Unauthorized** - Missing or invalid authentication token
- **403 Forbidden** - Insufficient permissions for the requested operation
- **404 Not Found** - Requested resource does not exist
- **500 Internal Server Error** - Server-side errors with detailed logging

**Error Response Format**:
```json
{
  "errorId": "123e4567-e89b-12d3-a456-426614174000",
  "message": "An error occurred while processing your request"
}
```

## 📦 NuGet Packages

| Package | Version | Purpose |
|---------|---------|---------|
| `AutoMapper` | 12.0.1 | Object-to-object mapping |
| `AutoMapper.Extensions.Microsoft.DependencyInjection` | 12.0.1 | AutoMapper DI integration |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 8.0.19 | JWT authentication |
| `Microsoft.AspNetCore.Identity.EntityFrameworkCore` | 8.0.19 | Identity management |
| `Microsoft.EntityFrameworkCore.SqlServer` | 9.0.8 | SQL Server data provider |
| `Microsoft.EntityFrameworkCore.Tools` | 9.0.8 | EF Core CLI tools |
| `Microsoft.IdentityModel.Tokens` | 8.14.0 | Token validation |
| `Serilog` | 4.3.0 | Structured logging framework |
| `Serilog.AspNetCore` | 9.0.0 | ASP.NET Core integration |
| `Serilog.Sinks.Console` | 6.0.0 | Console logging output |
| `Serilog.Sinks.File` | 7.0.0 | File logging output |
| `Swashbuckle.AspNetCore` | 6.6.2 | Swagger/OpenAPI documentation |
| `System.IdentityModel.Tokens.Jwt` | 8.14.0 | JWT token handling |

## ⚡ Quick Install

Install all required packages at once:

```bash
dotnet add package AutoMapper --version 12.0.1
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.19
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.19  
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.8
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.8
dotnet add package Microsoft.IdentityModel.Tokens --version 8.14.0
dotnet add package Serilog --version 4.3.0
dotnet add package Serilog.AspNetCore --version 9.0.0
dotnet add package Serilog.Sinks.Console --version 6.0.0
dotnet add package Serilog.Sinks.File --version 7.0.0
dotnet add package Swashbuckle.AspNetCore --version 6.6.2
dotnet add package System.IdentityModel.Tokens.Jwt --version 8.14.0
```

## 🛡️ Authentication & Security

### JWT Authentication

1. **Register** a new user via `/api/Auth/Register`
2. **Login** with credentials via `/api/Auth/Login` to receive a JWT token
3. **Include the token** in the Authorization header for protected endpoints:
   ```
   Authorization: Bearer your-jwt-token-here
   ```

### User Roles

- **Reader**: Can view walks and regions (GET operations)
- **Writer**: Full access to all operations (GET, POST, PUT, DELETE)

### Password Requirements

- Minimum 6 characters
- No complexity requirements (configurable in `Program.cs`)

## 📖 Project Architecture

```
NZWalks API/
├── Controllers/          # API controllers
├── Data/                # Database contexts
├── Models/
│   ├── Domain/          # Entity models
│   └── DTOs/            # Data transfer objects
├── Repositories/        # Data access layer
├── Middlewares/         # Custom middleware
├── CustomActionFilters/ # Validation filters
├── Mappings/           # AutoMapper profiles
├── Migrations/         # Database migrations
└── Images/             # Uploaded image storage
```

The project follows **Clean Architecture** principles with:
- **Repository Pattern** for data access
- **Dependency Injection** for loose coupling
- **DTO Pattern** for API contracts
- **Middleware** for cross-cutting concerns

## 📊 Logging & Monitoring

The application uses **Serilog** for structured logging with:

- **Console Output** for development
- **File Logging** with daily rolling files (`Logs/NzWalks_log.txt`)
- **Contextual Information** including user actions and request data
- **Error Tracking** with unique error IDs

Log levels configured for optimal debugging and monitoring.

## 📜 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

**Made with ❤️ for the New Zealand hiking community**