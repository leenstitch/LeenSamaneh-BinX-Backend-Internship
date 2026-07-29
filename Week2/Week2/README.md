# Week 2 - Advanced C# & .NET Fundamentals

## Overview
During Week 2, I worked on advanced C# concepts and ASP.NET Core API fundamentals.

## Day 1 - Generics & Advanced Collections
Topics covered:
- Generic classes and methods
- Generic constraints (where T : class)
- Repository pattern using Repository<T>
- IReadOnlyList<T>
- Predicate-based searching

Implementation:
- Created a reusable generic repository.
- Applied the repository with different models:
  - Book
  - Customer


## Day 2 - LINQ & Collections
Topics covered:
- GroupBy
- Join
- SelectMany
- Deferred Execution

Implementation:
- Grouped orders by customer.
- Joined customers with orders.
- Flattened order items using SelectMany.
- Demonstrated deferred execution behavior.


## Day 3 - Async/Await & Concurrency
Topics covered:
- Async methods
- Task.Delay simulation
- Sequential execution
- Concurrent execution using Task.WhenAll
- CancellationToken

Implementation:
- Created asynchronous data loading methods.
- Compared sequential and concurrent execution times.
- Added cancellation support.


## Day 4 - ASP.NET Core API Setup & Routing
Topics covered:
- Creating ASP.NET Core Web API project
- Minimal hosting model
- Controllers
- Minimal APIs
- Routing
- HTTP verbs
- Swagger testing
- Postman API collection

Implementation:
- Created Books Controller.
- Added GET all endpoint.
- Added GET by ID endpoint.
- Implemented the same endpoints using Minimal APIs.
- Tested endpoints using Postman.

## Technologies Used
- C#
- .NET 9
- ASP.NET Core Web API
- LINQ
- Async/Await
- Swagger
- Postman
- Git & GitHub
