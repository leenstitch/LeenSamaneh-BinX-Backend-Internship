# Day 4 — Centralized Error Handling & Global Exception Middleware

## Overview
Implemented centralized error handling in the Cardiac Patient Monitoring System using custom middleware.

## What I Learned
- Why scattered `try/catch` blocks are inefficient.
- How global exception-handling middleware works.
- Using `ProblemDetails` for standardized API errors.
- Structured logging with `ILogger`.

## Implementation
- Created `GlobalExceptionMiddleware`.
- Added centralized exception handling to the request pipeline.
- Returned safe `500 Internal Server Error` responses.
- Added structured logging with request path information.
- Added a test endpoint to trigger an exception and verify the middleware.
<img width="757" height="546" alt="image" src="https://github.com/user-attachments/assets/007bafa1-8e2b-48b1-8efc-e4b4a476b6e0" />

## Result
The API now handles unexpected exceptions consistently without exposing internal error details or stack traces.
