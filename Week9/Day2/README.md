# LensBook API

LensBook is a backend REST API for a photography booking platform.
The system allows customers to register, authenticate, browse photographers, and create and manage photography bookings.

The API is built using **ASP.NET Core .NET 9** and follows a layered architecture using Controllers, Services, Repositories, DTOs, Entity Framework Core, and SQL Server.

---

## Features

* Customer registration and authentication
* JWT-based authentication
* Refresh token authentication
* Role-based authorization
* Customer, Photographer, and Studio Owner roles
* Photography session bookings
* Booking status management
* Entity Framework Core database access
* SQL Server database
* Redis caching
* Swagger / OpenAPI documentation
* Postman API collection
* Global exception handling
* Correlation ID support
* Request logging
* Data validation

---

# Tech Stack

* **.NET 9**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **SQL Server**
* **ASP.NET Core Identity**
* **JWT Bearer Authentication**
* **Refresh Tokens**
* **Redis**
* **Swagger / OpenAPI**
* **Swashbuckle.AspNetCore**
* **Swashbuckle.AspNetCore.Filters**
* **Postman**
* **C#**

---

# Project Architecture

The project follows a layered architecture:

```text
LensBook
│
├── Controllers
│   └── API endpoints
│
├── Services
│   └── Business logic
│
├── Repositories
│   └── Database access
│
├── DTOs
│   └── Request and response models
│
├── Models
│   └── Entity models
│
├── DATA
│   ├── ApplicationDbContext
│   └── Seed
│
├── Middleware
│   ├── Exception Handling
│   ├── Correlation ID
│   └── Request Logging
│
├── Extensions
│   └── Application service and authentication configuration
│
└── Examples
    └── Swagger request and response examples
```

---

# Prerequisites

Before running the project, make sure the following are installed:

* .NET 9 SDK
* SQL Server
* Redis
* Visual Studio or Visual Studio Code
* Git
* Postman (optional, for testing the API)

---

# Setup

## 1. Clone the repository

```bash
git clone https://github.com/leenstitch/LeenSamaneh-BinX-Backend-Internship.git
```

Navigate to the LensBook project:

```bash
cd LeenSamaneh-BinX-Backend-Internship/Week9/Day1/LensBook
```

---

## 2. Restore dependencies

Run:

```bash
dotnet restore
```

This restores all NuGet packages required by the project.

---

## 3. Configure the database

The application uses **SQL Server**.

Update the database connection string in:

```text
appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=LensBookDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Replace `YOUR_SERVER` with your SQL Server instance.

---

## 4. Configure Redis

LensBook uses Redis for caching.

Configure the Redis connection string:

```json
{
  "ConnectionStrings": {
    "Redis": "localhost:6379"
  }
}
```

If Redis is running through Docker, make sure the Redis container is running before starting the application.

Example:

```bash
docker run --name lensbook-redis -p 6379:6379 -d redis
```

---

# Database Migrations

The project uses **Entity Framework Core Code First** migrations.

## 1. Install EF Core CLI

If the EF Core CLI is not installed:

```bash
dotnet tool install --global dotnet-ef
```

If it is already installed, update it when necessary:

```bash
dotnet tool update --global dotnet-ef
```

---

## 2. Create a migration

From the LensBook project directory:

```bash
dotnet ef migrations add InitialCreate
```

Use a different migration name when adding future changes, for example:

```bash
dotnet ef migrations add AddBookingStatus
```

---

## 3. Apply migrations

Run:

```bash
dotnet ef database update
```

This creates or updates the database according to the EF Core migration history.

---

# Running the Application

Run the API using:

```bash
dotnet run
```

The application will start using the configured HTTP/HTTPS URLs.

The exact URL can also be found in the application output when the project starts.

---

# API Documentation

LensBook provides interactive API documentation through **Swagger / OpenAPI**.

After running the application, open:

```text
https://localhost:<port>/swagger
```

Swagger provides:

* Available endpoints
* HTTP methods
* Request parameters
* Request bodies
* Response models
* HTTP status codes
* Authentication information
* Example requests and responses

The project also includes XML documentation comments for important endpoints and realistic Swagger examples.

---

# Authentication

LensBook uses **JWT Bearer Authentication**.

Authentication flow:

```text
Register
   ↓
Login
   ↓
Access Token + Refresh Token
   ↓
Access protected endpoints using Access Token
   ↓
Refresh Access Token when needed
```

For protected endpoints, include the access token in the request:

```text
Authorization: Bearer <access-token>
```

---

# User Roles

The system supports the following roles:

| Role         | Description                                                    |
| ------------ | -------------------------------------------------------------- |
| Customer     | Can register, log in, and create/manage bookings               |
| Photographer | Can authenticate and access photographer-related functionality |
| StudioOwner  | Administrative/studio owner functionality                      |

Role-based authorization is implemented using ASP.NET Core Identity and JWT claims.

---

# Environment Variables

The project uses configuration values for environment-specific settings.

Important configuration values include:

* Database connection string
* Redis connection string
* JWT issuer
* JWT audience
* JWT secret key
* JWT access-token expiration
* Refresh-token expiration

For security, sensitive values such as database passwords and JWT secret keys should not be committed to the repository.

For local development, sensitive configuration can be stored using:

* `appsettings.Development.json`
* .NET User Secrets
* Environment variables

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_DATABASE_CONNECTION_STRING",
    "Redis": "localhost:6379"
  },
  "Jwt": {
    "Issuer": "LensBook",
    "Audience": "LensBookUsers",
    "Key": "YOUR_SECRET_KEY"
  }
}
```

Replace placeholder values with the appropriate local configuration.

---

# Postman

A Postman collection is included for testing the API endpoints.

The collection contains organized requests for authentication and booking workflows.

Example structure:

```text
Week9-Day2
│
├── Auth
│   └── Happy Cases
│       ├── Admin Login
│       ├── Customer Login
│       ├── Photographer Login
│       ├── Register Customer
│       └── Refresh Token
│
└── Booking
    └── Happy Cases
        ├── Book a Session
        └── Update Booking Status
```

The Postman requests use environment variables such as:

```text
{{baseUrl}}
{{CustomerAccessToken}}
{{PhotographerAccessToken}}
{{AdminAccessToken}}
```

Postman test scripts are also included to verify the expected HTTP status code for each endpoint.

Examples:

```javascript
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});
```

For booking creation:

```javascript
pm.test("Status code is 201", function () {
    pm.response.to.have.status(201);
});
```

---

# Main API Endpoints

## Authentication

| Method | Endpoint                          | Description                   |
| ------ | --------------------------------- | ----------------------------- |
| POST   | `/api/v1/AuthController/register` | Register a new customer       |
| POST   | `/api/v1/AuthController/login`    | Login                         |
| POST   | `/api/v1/AuthController/refresh`  | Refresh authentication tokens |

## Booking

| Method | Endpoint                             | Description           |
| ------ | ------------------------------------ | --------------------- |
| POST   | `/api/v1/Booking`                    | Create a new booking  |
| PATCH  | `/api/v1/Booking/{bookingId}/status` | Update booking status |

---

# HTTP Status Codes

The API uses standard HTTP status codes.

| Status Code | Meaning                              |
| ----------- | ------------------------------------ |
| 200         | Request completed successfully       |
| 201         | Resource created successfully        |
| 400         | Invalid request or validation error  |
| 401         | Authentication required or invalid   |
| 403         | Authenticated user is not authorized |
| 404         | Requested resource was not found     |
| 500         | Unexpected server error              |

---

# API Documentation Link

Swagger documentation is available locally when the application is running:

```text
https://localhost:<port>/swagger
```

Replace `<port>` with the HTTPS port shown when the application starts.

---

# Development Notes

When developing or modifying the project:

1. Create or update the required EF Core migration.
2. Apply the migration to the local database.
3. Run the API.
4. Verify the endpoint using Swagger or Postman.
5. Check the expected HTTP status code.
6. Update the Postman collection when adding or modifying endpoints.
7. Update Swagger examples and XML documentation for important endpoints.
8. Keep environment-specific and sensitive configuration out of source control.

---

# Security Notes

Do not commit real:

* JWT secret keys
* Database passwords
* Access tokens
* Refresh tokens
* Production connection strings
* Personal credentials

Use environment variables or .NET User Secrets for sensitive configuration.

---

# Author

**Leen Samaneh**

Computer Systems Engineering
Palestine Technical University – Kadoorie

GitHub: `https://github.com/leenstitch`

LinkedIn: `https://linkedin.com/in/leen-samaneh/`
