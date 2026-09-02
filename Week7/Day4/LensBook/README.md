# Day 4 — Custom Middleware & Cross-Cutting Concerns

## Overview

Day 4 focused on implementing **custom middleware** to handle cross-cutting concerns in the LensBook API .

The implemented middleware provides centralized exception handling, request logging with execution time, and correlation IDs for request tracing.

## What I Completed

### 1. Global Exception Handling Middleware

Implemented `ExceptionHandlingMiddleware` to handle unhandled exceptions centrally instead of handling errors separately inside every controller.

It:

* Catches unhandled exceptions.
* Logs the exception.
* Returns a consistent JSON response.
* Maps common exception types to appropriate HTTP status codes.
* Returns `500 Internal Server Error` for unexpected exceptions.
<img width="1317" height="500" alt="Screenshot 2026-09-02 094723" src="https://github.com/user-attachments/assets/d81806bd-0e2b-40c7-932c-ba4823f1e4e0" />

### 2. Request Logging & Timing Middleware

Implemented `RequestLoggingMiddleware` to log API requests globally.

It records:

* HTTP method.
* Request path.
* Response status code.
* Request execution time.
* Correlation ID.
<img width="1692" height="36" alt="Screenshot 2026-09-02 094604" src="https://github.com/user-attachments/assets/183ce75a-efaa-4376-b182-9b46a4b68e83" />

This allows monitoring API requests without adding logging code to every controller.

### 3. Correlation ID Middleware

Implemented `CorrelationIdMiddleware` to assign a unique identifier to each request.

The correlation ID:

* Is generated when the request does not already contain one.
* Is returned in the response headers using `X-Correlation-ID`.
* Is included in request logs.
* Helps trace a specific request across the API logs.
<img width="1315" height="660" alt="Screenshot 2026-09-02 094617" src="https://github.com/user-attachments/assets/9857515f-b036-4b1c-81e9-25e4f3062801" />

### 4. Middleware Pipeline Configuration

Registered the custom middleware in Program.cs so that the functionality is applied globally across the API.

The pipeline was configured as:

Exception Handling
        ↓
Correlation ID
        ↓
Request Logging & Timing
        ↓
Authentication
        ↓
Authorization
        ↓
Controllers


### 5. Testing

Tested the middleware across multiple API endpoints to verify that the functionality is applied consistently without requiring changes inside individual controllers.

Verified:

* Correlation IDs are returned with requests.
* Requests are logged with their execution time.
* Exceptions are handled centrally.
* Different HTTP status codes are returned for supported exception types.

### 6. Git & Pull Request Preparation

* Updated the local main branch from GitHub.
* Created a Sprint2 branch.
* Added and committed the Sprint 2 changes.
* Pushed the Sprint2 branch to GitHub.
* Prepared the branch for Pull Request and mentor review.


## Key Learning

The main focus of Day 4 was understanding how **cross-cutting concerns** can be handled centrally using middleware instead of duplicating the same logic across multiple controllers and endpoints.

