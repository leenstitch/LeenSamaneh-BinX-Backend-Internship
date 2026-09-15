# LensBook — Day 3: Building the CI/CD Pipeline

![Build and Test](https://github.com/leenstitch/LeenSamaneh-BinX-Backend-Internship/actions/workflows/build-and-test.yml/badge.svg)

## Overview

Day 3 focused on building a basic **CI (Continuous Integration) pipeline** for the LensBook backend using **GitHub Actions**.

The goal was to make sure that every push or pull request automatically:

1. Restores the project dependencies.
2. Builds the solution.
3. Runs the automated tests.
4. Fails the pipeline when any test fails.

This provides an automatic quality check for the project instead of relying only on local testing.

---

## Project

**LensBook** is a photography booking backend API built with:

* ASP.NET Core
* .NET 9
* C#
* Entity Framework Core
* SQL Server
* ASP.NET Core Identity
* JWT Authentication
* Refresh Tokens
* xUnit
* Moq
* Swagger / OpenAPI

The Day 3 CI pipeline was created specifically to automatically build and test this project.

---

## Day 3 Objectives

The main objectives were:

* Learn GitHub Actions workflow syntax.
* Create an automated build and test pipeline.
* Understand workflow triggers.
* Understand jobs and steps.
* Run the pipeline automatically on `push`.
* Run the pipeline automatically on `pull_request`.
* Make sure the pipeline fails when a test fails.
* Verify both failure and success scenarios.
* Add a CI status badge to the README.

---

## GitHub Actions

The CI workflow is located at:

```text
.github/workflows/build-and-test.yml
```

GitHub Actions automatically detects workflow files placed inside:

```text
.github/workflows/
```

---

## Workflow Structure

The workflow contains:

```text
Workflow
   │
   └── Job: build
        │
        ├── Checkout code
        ├── Setup .NET
        ├── Restore dependencies
        ├── Build solution
        └── Run tests
```

---

## Workflow Triggers

The workflow is triggered by both:

```yaml
on:
  push:
  pull_request:
```

### Push

Whenever code is pushed to the repository, GitHub Actions automatically starts the pipeline.

### Pull Request

Whenever a pull request is created or updated, the pipeline also runs.

This helps detect problems before code is merged.

---

## CI Workflow

The complete workflow is:

```yaml
name: build-and-test

on:
  push:
  pull_request:

jobs:
  build:
    runs-on: ubuntu-latest

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'

      - name: Restore dependencies
        run: dotnet restore Week9/Day3/LensBook/LensBook.sln

      - name: Build
        run: dotnet build Week9/Day3/LensBook/LensBook.sln --no-restore

      - name: Test
        run: dotnet test Week9/Day3/LensBook/LensBook.sln --no-build --no-restore
```

---

## Workflow Steps

### 1. Checkout Code

```yaml
- name: Checkout code
  uses: actions/checkout@v4
```

This downloads the repository code onto the GitHub Actions runner.

Without this step, the runner would not have access to the project files.

---

### 2. Setup .NET

```yaml
- name: Setup .NET
  uses: actions/setup-dotnet@v4
  with:
    dotnet-version: '9.0.x'
```

This installs and configures the required .NET 9 SDK on the GitHub runner.

The project uses .NET 9, so the CI environment must use a compatible SDK.

---

### 3. Restore Dependencies

```yaml
- name: Restore dependencies
  run: dotnet restore Week9/Day3/LensBook/LensBook.sln
```

This restores all NuGet dependencies required by the solution.

The solution contains both:

```text
LensBook
LensBookTests
```

Using the solution ensures that the dependencies for the complete solution are restored.

---

### 4. Build

```yaml
- name: Build
  run: dotnet build Week9/Day3/LensBook/LensBook.sln --no-restore
```

This builds the solution.

The `--no-restore` option means that the restore step is not repeated because dependencies were already restored in the previous step.

If the project contains compilation errors, this step fails and the pipeline stops.

---

### 5. Test

```yaml
- name: Test
  run: dotnet test Week9/Day3/LensBook/LensBook.sln --no-build --no-restore
```

This runs the automated tests in the solution.

The `--no-build` option means the solution does not need to be built again because it was already built in the previous step.

The `--no-restore` option prevents another unnecessary restore operation.

---

## Failing Fast

One of the main objectives of Day 3 was to verify that the CI pipeline actually fails when a test fails.

A test was intentionally modified so that the expected value was incorrect.

The test expected:

```text
Expected: 50
Actual:   5
```

The local test result became:

```text
Failed:     1
Passed:    48
Skipped:    0
Total:     49
```

Because `dotnet test` returned a non-zero exit code, GitHub Actions detected the failure.

The pipeline result was:

```text
Status: Failure
Process completed with exit code 1.
```

This confirmed that a failing automated test correctly causes the CI pipeline to fail.

---

## Fixing the Failed Test

After verifying the failure scenario, the incorrect assertion was fixed.

The test was changed back to the correct expected value:

```csharp
Assert.Equal(
    5,
    result.PhotographerId);
```

The tests were then executed locally again.

The final local test result was:

```text
Passed:    49
Failed:     0
Skipped:    0
Total:     49
```

The corrected changes were pushed to GitHub.

GitHub Actions automatically started a new workflow run and the pipeline completed successfully.

---

## CI Results

The pipeline was tested in two scenarios.

### Failure Scenario

```text
49 Total Tests
48 Passed
1 Failed
```

Result:

```text
🔴 GitHub Actions — Failure
```

This confirmed that the pipeline correctly detects test failures.

### Success Scenario

After fixing the test:

```text
49 Total Tests
49 Passed
0 Failed
0 Skipped
```

Result:

```text
🟢 GitHub Actions — Success
```

This confirmed that the pipeline succeeds when the code and tests are correct.

---

## CI Pipeline Flow

The final CI process is:

```text
Developer pushes code
        │
        ▼
GitHub Actions triggered
        │
        ▼
Checkout repository
        │
        ▼
Setup .NET 9
        │
        ▼
Restore dependencies
        │
        ▼
Build solution
        │
        ▼
Run tests
        │
        ├───────────────┐
        │               │
     Tests pass      Test fails
        │               │
        ▼               ▼
   🟢 Success        🔴 Failure
```

---

## Status Badge

A GitHub Actions status badge was added at the top of this README:

```markdown
![Build and Test](https://github.com/leenstitch/LeenSamaneh-BinX-Backend-Internship/actions/workflows/build-and-test.yml/badge.svg)
```

The badge provides a quick indication of whether the latest workflow run is passing or failing.

---

## Repository Structure

The relevant Day 3 structure is:

```text
.github/
└── workflows/
    └── build-and-test.yml

Week9/
└── Day3/
    └── LensBook/
        ├── LensBook.sln
        ├── LensBook/
        │   └── LensBook.csproj
        │
        └── LensBookTests/
            └── LensBookTests.csproj
```

---

## What I Learned

During Day 3, I learned how to:

* Create a GitHub Actions workflow using YAML.
* Understand the structure of a workflow.
* Define jobs and steps.
* Configure a .NET environment on a GitHub runner.
* Restore NuGet dependencies automatically.
* Build a .NET solution automatically.
* Run automated tests in CI.
* Configure workflows to run on pushes and pull requests.
* Understand how exit codes affect CI results.
* Verify that a failing test makes the pipeline fail.
* Fix the failing test and verify that the pipeline becomes successful again.
* Add a GitHub Actions status badge to a project README.

---

## Final Result

The LensBook backend now has an automated CI pipeline that verifies the project whenever code is pushed or a pull request is created.

The pipeline automatically:

```text
Checkout
   ↓
Setup .NET 9
   ↓
Restore
   ↓
Build
   ↓
Test
   ↓
🟢 Pass / 🔴 Fail
```

This provides an automated safety check and helps prevent broken code from passing unnoticed.

