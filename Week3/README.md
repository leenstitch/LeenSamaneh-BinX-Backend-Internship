# Week 3 Summary — REST API Development

During Week 3, I designed and implemented a RESTful API using ASP.NET Core Web API. 
The main focus was building a structured API with proper resource modeling, database integration, CRUD operations, and API testing.

## What I Completed

- Designed REST API resources and followed RESTful naming conventions.
- Created Entity models and configured Entity Framework Core with SQL Server.
- Created DbContext and managed database migrations.
- Implemented full CRUD operations:
  - Create resources
  - Get all resources
  - Get resource by ID
  - Update resources
  - Delete resources
- Added proper HTTP status code handling:
  - 200 OK
  - 201 Created
  - 204 No Content
  - 400 Bad Request
  - 404 Not Found
- Tested all API endpoints using Postman.
- Created a Postman collection containing:
  - Successful requests
  - Error scenarios
  - Automated response validation tests
- Configured a Postman environment using a `baseUrl` variable to make API requests easier to manage.

## Tools & Technologies

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- REST API
- Postman
- HTTP Status Codes

## Outcome

By the end of Week 3, I built and tested a complete RESTful API with database integration, implemented CRUD functionality, and prepared API documentation/testing through Postman.
