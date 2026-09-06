## SPRINT 3 — ADVANCED QUERIES & PERFORMANCE

### Sprint Goal

Improve the performance of the LensBook API by analyzing EF Core queries, identifying inefficient database access patterns, and applying optimized queries, pagination, Redis caching, and performance measurements.

### Performance Target

Reduce unnecessary database queries and improve the response time of the main list endpoints while keeping the API behavior unchanged.

---

# Day 1 — Sprint Planning & N+1 Diagnosis

### Goals

* Define the Sprint 3 goal and performance target.
* Understand the **N+1 Query Problem**.
* Enable EF Core query logging.
* Test important list endpoints and inspect the actual SQL queries.

### Tasks

* Review the Sprint 2 retrospective and carry its improvement action into Sprint 3.
* Enable EF Core SQL logging.
* Test 2–3 important list endpoints.
* Count the actual database queries generated.
* Check whether any endpoint has an N+1 problem.
* Document the results.

### Result

The tested endpoints did not show a genuine N+1 problem. The `My Bookings` endpoint uses `JOINs` through `Include`, while the other tested endpoints use a single query.

---

# Day 2 — Advanced LINQ & Query Optimization

### Goals

* Improve the way data is retrieved from the database.
* Avoid loading unnecessary data.
* Use more efficient LINQ queries.

### Tasks

* Review existing LINQ queries.
* Use `Select` to retrieve only the required fields.
* Compare loading full entities with loading only required data.
* Review unnecessary `Include` statements.
* Check filtering and sorting at the database level.
* Compare the generated SQL before and after optimization.

### Result

The main queries are optimized to retrieve only the data required by the API response.

---

# Day 3 — Pagination & Large Data Sets

### Goals

* Prevent large datasets from being returned in a single response.
* Implement pagination for important list endpoints.

### Tasks

* Add `pageNumber` and `pageSize` parameters.
* Use `Skip()` and `Take()`.
* Return paginated results through a suitable DTO.
* Test different page sizes.
* Check the generated SQL query.

### Example

var bookings = await _context.Bookings
    .Where(b => b.PhotographerId == photographerId)
    .OrderBy(b => b.StartTime)
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();


### Result

Large datasets are divided into smaller pages instead of returning everything at once.

---

# Day 4 — Redis Caching

### Goals

* Understand why caching can improve API performance.
* Add Redis caching for suitable read-heavy endpoints.

### Tasks

* Configure Redis.
* Identify endpoints whose data does not change frequently.
* Store frequently requested results in Redis.
* Return cached data when available.
* Remove or update cached data when the underlying data changes.
* Test the endpoint with and without caching.

### Result

Frequently requested data can be returned from the cache instead of querying the database every time.

---

# Day 5 — Performance Testing & Sprint Review

### Goals

* Measure the improvements made during Sprint 3.
* Compare performance before and after optimization.
* Review the Sprint work and document the results.

### Tasks

* Test the main endpoints again.
* Compare response times.
* Compare database query counts.
* Check whether pagination reduced the amount of returned data.
* Check whether caching reduced database requests.
* Document the performance results.
* Review completed and incomplete Sprint tasks.
* Write the Sprint 3 retrospective.

### Final Sprint Review

At the end of the Sprint, compare:

| Metric           |   Before |    After |
| ---------------- | -------: | -------: |
| Database Queries | Measured | Measured |
| Response Time    | Measured | Measured |
| Response Size    | Measured | Measured |
| Cached Requests  |        0 | Measured |

### Sprint 3 Outcome

By the end of Sprint 3, the API should have:

* Better-performing EF Core queries.
* No unnecessary N+1 queries in the tested endpoints.
* Pagination for large list endpoints.
* Redis caching for suitable endpoints.
* Measured performance improvements.
* Documented results and Sprint retrospective.


----------------------------------------------------------------------------------------------------------
#### DAY 1 :
# EF Core Query Logging & N+1 Diagnosis

We enabled **EF Core Query Logging** to see the actual SQL queries executed by our API and check for the **N+1 Problem**.

---

## 1. My Bookings

### Repository Code

public async Task<IEnumerable<Booking>> GetMyBookingsAsync(int photographerId)
{
    return await _context.Bookings
        .Include(b => b.Customer)
        .Include(b => b.SessionType)
        .Where(b => b.PhotographerId == photographerId)
        .OrderBy(b => b.StartTime)
        .ToListAsync();
}

We use `Include` to load the related Customer and SessionType data with the bookings.

### SQL Query


SELECT [b].[BookingId], [b].[CustomerId], [b].[EndTime],
       [b].[Notes], [b].[PhotographerId], [b].[SessionTypeId],
       [b].[StartTime], [b].[Status],
       [c].[CustomerId], [c].[FirstName], [c].[LastName],
       [s].[SessionTypeId], [s].[Name]
FROM [Bookings] AS [b]
INNER JOIN [Customers] AS [c]
    ON [b].[CustomerId] = [c].[CustomerId]
INNER JOIN [SessionTypes] AS [s]
    ON [b].[SessionTypeId] = [s].[SessionTypeId]
WHERE [b].[PhotographerId] = @__photographerId_0
ORDER BY [b].[StartTime]


The `Include` statements are translated into **JOINs**, so EF Core does not execute a separate query for each booking.

There is also one query to get the Photographer, so the request executed **2 queries in total**.

**Result: No N+1 Problem.**

---

## 2. Session Types

### Repository Code


public async Task<IEnumerable<SessionType>> GetAllAsync()
{
    return await _context.SessionTypes
        .ToListAsync();
}


### SQL Query


SELECT [s].[SessionTypeId],
       [s].[Description],
       [s].[DurationInMinutes],
       [s].[IsActive],
       [s].[Name],
       [s].[Price]
FROM [SessionTypes] AS [s]


All Session Types are retrieved using **one query only**.

**Result: No N+1 Problem.**

---

## 3. Notifications

### SQL Query


SELECT [n].[NotificationId],
       [n].[CreatedAt],
       [n].[IsRead],
       [n].[Message],
       [n].[Title],
       [n].[UserId]
FROM [Notifications] AS [n]
WHERE [n].[UserId] = @__userId_0
ORDER BY [n].[CreatedAt] DESC


All notifications for the user are retrieved using **one query only**.

**Result: No N+1 Problem.**

---

## Final Result

| Endpoint      | Queries | N+1 |
| ------------- | ------: | --- |
| My Bookings   |       2 | No  |
| Session Types |       1 | No  |
| Notifications |       1 | No  |

After checking the actual SQL queries, we did **not find a genuine N+1 Problem** in the tested endpoints.


### Test Data / Seeding

The task required preparing **seed data with at least 50 records** in the main tables.
Since the project database already contains **more than 50 realistic records**, additional seeding was not necessary.




