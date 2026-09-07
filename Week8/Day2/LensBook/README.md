## Query Optimization Results

For the `GetMyBookings` endpoint, I compared the original implementation using `Include` with a projection-based implementation using `Select`.

### Before — Eager Loading with Include

The original query used:

```csharp
.Include(b => b.Customer)
.Include(b => b.SessionType)
```

This caused EF Core to generate a SQL query containing `JOIN`s with the `Customers` and `SessionTypes` tables and retrieve additional columns from those related entities.

The endpoint executed 2 database queries:

1. Retrieve the photographer profile.
2. Retrieve the photographer's bookings with related Customer and SessionType data.

### After — Projection

The endpoint was changed to use:

```csharp
.Select(b => new BookingResponseDto
{
    BookingId = b.BookingId,
    CustomerId = b.CustomerId,
    PhotographerId = b.PhotographerId,
    SessionTypeId = b.SessionTypeId,
    StartTime = b.StartTime,
    EndTime = b.EndTime,
    Status = b.Status.ToString(),
    Notes = b.Notes
})
```

The endpoint still executes 2 database queries, but the bookings query now selects only the fields required by `BookingResponseDto`.

The generated SQL no longer contains unnecessary `JOIN`s with `Customers` and `SessionTypes`.

### Result

| Approach   | Queries | Related JOINs          | Data Retrieved       |
| ---------- | ------: | ---------------------- | -------------------- |
| Include    |       2 | Customer + SessionType | More data            |
| Projection |       2 | None                   | Only required fields |

Therefore, projection provided a leaner query by reducing unnecessary data retrieval, even though the total number of database queries remained the same.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

### AsSplitQuery Experiment

To demonstrate `AsSplitQuery()`, I created a test endpoint that loads a photographer together with two collection navigation properties:

* `Bookings`
* `ExternalSchedules`

The query was implemented using:

```csharp
var photographer = await _context.Photographers
    .Include(p => p.Bookings)
    .Include(p => p.ExternalSchedules)
    .AsSplitQuery()
    .FirstOrDefaultAsync(
        p => p.PhotographerId == photographerId);
```

### Result

EF Core generated three separate SQL queries:

1. One query to retrieve the photographer.
2. One query to retrieve the photographer's bookings.
3. One query to retrieve the photographer's external schedules.

The logs showed approximately:

* Photographer query: **12 ms**
* Bookings query: **12 ms**
* ExternalSchedules query: **32 ms**

The complete request finished in approximately **306 ms**.

This confirmed that `AsSplitQuery()` successfully split the loading of multiple collection navigations into separate database queries.

### Why We Used It

Loading multiple collections in a single query can produce a large number of duplicated rows because of joins between the collections, potentially causing a Cartesian explosion.

`AsSplitQuery()` avoids this by loading each collection separately.

The measured response time was also recorded, but it should not be used alone to claim a performance improvement because request time can be affected by application 
startup, debugging, middleware, caching, and other factors.

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

## Day 2 — Query Optimization with Eager & Explicit Loading

Today, we focused on optimizing EF Core queries and reducing unnecessary data retrieval.

### What We Worked On

* Reviewed **Eager Loading** using `Include` and `ThenInclude` for loading related data.
* Applied **Projection** using `Select` to return only the fields required by the DTO instead of loading complete related entities.
* Compared the original `Include` approach with the projection-based approach by analyzing the generated SQL queries and query execution times.
* Created a test endpoint with multiple collection navigation properties to demonstrate **Split Queries**.
* Applied `AsSplitQuery()` to load `Bookings` and `ExternalSchedules` separately and avoid potential Cartesian product issues caused by joining multiple collections.
* Used EF Core query logging to inspect the generated SQL and verify the behavior of the optimized queries.
* Documented the before/after results and confirmed that the optimization reduced unnecessary data retrieval.

### Key Takeaway

`Include` is useful when complete related entities are required, while **Projection** is more suitable for list and summary endpoints where only specific fields are needed. `AsSplitQuery()` can be useful when loading multiple collection relationships to avoid excessive data duplication caused by large JOIN operations.

