# Week 8 — Day 3: Introducing Redis Caching

## Overview

Day 3 focused on introducing caching into the LensBook API using **Redis** and ASP.NET Core's `IDistributedCache` abstraction.

The goal was to reduce unnecessary database queries for frequently requested data by storing a temporary copy of the data in Redis.

The implementation followed the **Cache-Aside Pattern**, where the application checks the cache first, retrieves data from the database only when the cache does not contain the requested data, and then stores the result in Redis for future requests.

---

## 1. What Belongs in a Cache?

Not all application data should be cached.

Good candidates for caching are usually:

* Read frequently.
* Changed relatively rarely.
* Expensive or unnecessary to retrieve repeatedly from the database.

For the LensBook project, **SessionTypes** were selected as a suitable caching candidate because they can be requested frequently and do not change constantly.

The endpoint used for the experiment was:

GET /api/v1/SessionTypes

<img width="769" height="751" alt="image" src="https://github.com/user-attachments/assets/b91e61cb-5c6f-420b-a9dd-4b860b212cde" />

Data such as frequently changing booking status or other highly dynamic information would be less suitable for this simple caching strategy.

---

## 2. Setting Up Redis with Docker

Redis was installed locally by running it inside a Docker container.

The Redis container was created using:

powershell
docker run -d --name lensbook-redis -p 6379:6379 redis:latest


The running container was verified using:

powershell
docker ps


The Redis container was successfully running with:

NAMES
lensbook-redis


and the port mapping:
0.0.0.0:6379->6379/tcp


This made Redis available to the application through:
localhost:6379

---

## 3. Installing the Redis Package

The following NuGet package was added to the LensBook project:

Microsoft.Extensions.Caching.StackExchangeRedis

This package provides the StackExchange.Redis-backed implementation of ASP.NET Core's `IDistributedCache`.

---

## 4. Registering IDistributedCache

Redis was registered in `Program.cs`:


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration
            .GetConnectionString("Redis");
});

The Redis connection string was added to `appsettings.json`:

"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=LensBookDb;Trusted_Connection=True;TrustServerCertificate=True;",
    "Redis": "localhost:6379"
}

This allows the application to use Redis through the framework-provided `IDistributedCache` abstraction instead of directly depending on Redis-specific code throughout the application.

---

## 5. Cache Invalidation

Caching introduces a data consistency problem when the database changes.

For example, if a new SessionType is added while the old list is still stored in Redis, Redis would contain outdated data.

To prevent this, the cache entry is removed when a SessionType is added.

The `AddAsync()` method was updated:

<img width="539" height="239" alt="image" src="https://github.com/user-attachments/assets/4acf0242-8b17-4ad5-8206-74299092b86c" />

The next `GET /SessionTypes` request will therefore be a cache miss, causing the application to retrieve the updated list from SQL Server and store the fresh result in Redis.

The current SessionType repository only contains an Add operation, so cache invalidation was implemented for the available write operation.

---

## 6. Testing Cache Hit and Cache Miss

The caching behavior was tested using the `GET /api/v1/SessionTypes` endpoint.

### First Request — Cache Miss

The first request queried SQL Server:

Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (80ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
SELECT [s].[SessionTypeId], [s].[Description], [s].[DurationInMinutes], [s].[IsActive], [s].[Name], [s].[Price]
FROM [SessionTypes] AS [s]

LensBook.Middleware.RequestLoggingMiddleware: Information: Request finished: GET /api/v1/SessionTypes - Status: 200 - Duration: 8732ms - CorrelationId: 339e672d-3158-4655-8f45-6d38e2b83f46

This request was a **Cache Miss**, because the data was not yet available in Redis.

---

### Second Request — Cache Hit

The same endpoint was called again immediately.

The request duration was:

LensBook.Middleware.RequestLoggingMiddleware: Information: Request started: GET /api/v1/SessionTypes - CorrelationId: 6b0a91f0-a479-40fc-91d5-99ea39257639

LensBook.Middleware.RequestLoggingMiddleware: Information: Request finished: GET /api/v1/SessionTypes - Status: 200 - Duration: 288ms - CorrelationId: 6b0a91f0-a479-40fc-91d5-99ea39257639

No `Executed DbCommand` appeared for the second request, confirming that the data was returned from Redis without executing another SQL query.

---

## 7. Cache Invalidation Test

A new SessionType was added after the cache had already been populated.

The `AddAsync()` operation removed:

sessiontypes:all

from Redis.

The `GET /api/v1/SessionTypes` endpoint was then called again.

The newly added SessionType appeared immediately in the response.

This confirmed that the cache invalidation logic was working correctly and that stale SessionType data was not returned after a write operation.

---

## 8. Performance Results

The measured results were:

| Request        | Cache State | Response Time |
| -------------- | ----------- | ------------: |
| First request  | Cache Miss  |       8732 ms |
| Second request | Cache Hit   |        288 ms |

The measured difference was:

8732 ms - 288 ms = 8444 ms

The second request was significantly faster in this specific experiment.

However, the full request duration includes application and middleware overhead, especially during development/debugging. Therefore, these measurements should be treated as experimental results rather than a general claim about Redis performance.

The more important observation was that the cache hit did not execute another SQL query.

---


## Conclusion

Day 3 successfully introduced Redis caching into the LensBook API.

Redis was configured locally using Docker, integrated with ASP.NET Core through `IDistributedCache`, and applied to the SessionTypes endpoint using the Cache-Aside Pattern.

The implementation was tested through cache miss, cache hit, and cache invalidation scenarios. The results confirmed that subsequent requests could be served from Redis without executing another database query, while changes to SessionTypes correctly invalidated the cached data.
