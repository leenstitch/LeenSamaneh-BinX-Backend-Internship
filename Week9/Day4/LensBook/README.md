# Week 9 — Day 4: Deployment & CI/CD

## Overview

In Day 4, the LensBook API was deployed to a production environment and a CI/CD pipeline was configured to automatically build, test, and deploy the application.

## Deployment

* Deployed the LensBook ASP.NET Core Web API to **Railway**.
* Configured the application to run using Docker.
* Configured the production environment variables in Railway.
* Configured the production SQL Server connection using **MonsterASP**.
* Configured Redis using **Upstash Redis**.
* Verified the deployed API through the public Swagger UI.
* Tested user registration successfully on the deployed API.

## Production Configuration

The following production configuration was managed through Railway environment variables:

* SQL Server connection string
* Redis connection
* JWT configuration/secrets

Sensitive values were kept outside the source code and managed through environment variables.

## GitHub Actions CI/CD

A GitHub Actions workflow was configured to run on pushes and pull requests.

### Build & Test Pipeline

The workflow performs:

1. Checkout the repository.
2. Setup .NET 9.
3. Restore dependencies.
4. Build the LensBook solution.
5. Run automated tests.

### Automated Deployment

A deployment job was added after the build and test job.

The deployment job:

* Runs only for pushes to the `main` branch.
* Waits for the build and tests to complete successfully.
* Uses a GitHub repository secret named `RAILWAY_TOKEN`.
* Deploys the LensBook API to the configured Railway service.

## CI/CD Flow

```text
Git Push
   ↓
GitHub Actions
   ↓
Restore
   ↓
Build
   ↓
Test
   ↓
Deploy to Railway
   ↓
Production API
```

## Railway CI Configuration

Railway's **Wait for CI** option was enabled so that deployment waits for the GitHub Actions CI pipeline to complete successfully.

## Verification

A real change was pushed to the `main` branch to verify the complete CI/CD pipeline.

The final pipeline completed successfully:

* Build ✅
* Test ✅
* Deploy to Railway ✅

The deployed API was also verified through Swagger after deployment.
