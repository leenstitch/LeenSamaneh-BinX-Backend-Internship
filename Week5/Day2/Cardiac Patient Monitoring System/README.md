# Day 2 — Mocking Dependencies with Moq

## Topics Covered

* Understanding why dependencies should be mocked in unit tests
* Setting up mocks using **Moq**
* Configuring mocked methods with `.Setup()`
* Mocking return values using `.ReturnsAsync()`
* Mocking exceptions using `.Throws()`
* Verifying dependency interactions using `.Verify()`
* Testing services in isolation from real repositories and databases

## Hands-On Lab

* Identified a service method that depends on a repository interface.
* Created a mock repository using `Mock<IRepository>`.
* Configured the mock repository to return specific test data.
* Tested the service logic using the mocked repository.
* Configured the mock to throw an exception and tested the service's exception handling.
* Used Moq's `.Verify()` to confirm that a repository method was called exactly once.
* Added and committed the unit tests to the GitHub repository.
<img width="741" height="712" alt="image" src="https://github.com/user-attachments/assets/cbad39f5-9a27-4f34-b99e-1d3a66efbd69" />


## Tools Used

* C#
* xUnit
* Moq
* .NET
* Visual Studio

## Key Learning

Learned how to isolate a service from its real dependencies by replacing repositories with controlled mocks. This allows unit tests to focus on the service's own logic without relying on a real database or external dependencies.
