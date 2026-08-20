# Week 5 — Testing, Error Handling & Project Kickoff

## Overview

Week 5 focused on improving API reliability through testing and centralized error handling, while preparing the project for Phase 3.

The week covered unit testing with **xUnit and Moq**, integration testing with **WebApplicationFactory**, centralized exception handling using **middleware and ProblemDetails**, and applying these practices to the Cardiac Patient Monitoring System.

## Learning Objectives

- Write unit tests using the Arrange-Act-Assert pattern.
- Mock dependencies using Moq.
- Write integration tests against real HTTP endpoints.
- Implement centralized exception handling.
- Use ProblemDetails for standardized API error responses.
- Apply structured logging with `ILogger`.
- Identify and test high-risk business logic.
- Run and interpret the complete test suite.

## Daily Work

### Day 1 — Unit Testing with xUnit
- Reviewed testing fundamentals and the Arrange-Act-Assert pattern.
- Created unit tests for important service logic.
- Focused on high-risk functionality rather than simple CRUD operations.

### Day 2 — Mocking with Moq
- Learned how to isolate services from repositories and external dependencies.
- Configured mocked return values and verified repository interactions.
- Applied Moq to service-layer testing.

### Day 3 — Integration Testing
- Set up `WebApplicationFactory`.
- Created an isolated test environment using an in-memory database.
- Added test authentication for protected endpoints.
- Tested the Patient Health Status endpoint through the real HTTP pipeline.

### Day 4 — Centralized Error Handling
- Implemented global exception-handling middleware.
- Added standardized `ProblemDetails` responses.
- Added structured logging using `ILogger`.
- Created a test endpoint to verify exception handling without exposing internal details.

### Day 5 — Testing the Project & Week Synthesis
- Identified high-risk areas in the Cardiac Patient Monitoring System.
- Added unit tests for:
  - Authentication
  - Vital Signs
  - Medications
- Added integration tests for Patient Health Status.
- Ran the complete test suite using `dotnet test`.

## Testing Results

```text
Total Tests: 46
Passed:      46
Failed:       0
Skipped:      0
