# Day 4 — Database Indexing & Performance Profiling

## Overview

Day 4 focused on improving database query performance through **database indexing** and measuring query behavior before and after applying indexes.

The main goal was not only to add indexes, but also to verify their effect using **real EF Core query execution logs**.

An index can improve read performance for frequently used queries, but unnecessary indexes also add overhead to database write operations. Therefore, indexes were selected based on actual query patterns in the LensBook API.

---

# 1. Identifying Index Candidates

The existing repositories were reviewed to identify queries that repeatedly filter, join, or sort using the same columns.

### Candidate 1 — Bookings

The `PhotographerRepository` contains the following query pattern:

return await _context.Bookings
    .Include(b => b.Customer)
    .Include(b => b.SessionType)
    .Where(b => b.PhotographerId == photographerId)
    .OrderBy(b => b.StartTime)
    .ToListAsync();

The query:

* Filters by `PhotographerId`
* Sorts by `StartTime`

Therefore, these two columns are used together frequently.

A composite index was selected: (PhotographerId, StartTime)

---

### Candidate 2 — Notifications

The `NotificationRepository` contains:

return await _context.Notifications
    .Where(n => n.UserId == userId)
    .OrderByDescending(n => n.CreatedAt)
    .ToListAsync();

The query:

* Filters by `UserId`
* Sorts by `CreatedAt DESC`

Therefore, a composite index was selected: (UserId, CreatedAt)

---

### Candidate 3 — Photographers.UserId

The following query is frequently used:

return await _context.Photographers
    .Include(p => p.User)
    .FirstOrDefaultAsync(p => p.UserId == userId);
    
`UserId` is therefore an index candidate.

However, after reviewing the existing database migration, it was found that `Photographers.UserId` already has a **unique index**:

migrationBuilder.CreateIndex(
    name: "IX_Photographers_UserId",
    table: "Photographers",
    column: "UserId",
    unique: true);

Therefore, no duplicate index was added.

This demonstrates that indexes should be reviewed based on the existing database schema instead of blindly adding new ones.

---

# 2. Composite Indexes

A composite index contains more than one column.

For the Bookings query, the following index was added:

modelBuilder.Entity<Booking>()
    .HasIndex(b => new { b.PhotographerId, b.StartTime });

For Notifications:

modelBuilder.Entity<Notification>()
    .HasIndex(n => new { n.UserId, n.CreatedAt });

The order of columns matters.

For example: (PhotographerId, StartTime)

is designed for queries that first identify a photographer and then work with the booking start times.

Similarly: (UserId, CreatedAt)

matches the notification query that first filters by user and then orders by creation date.

---

# 3. New Migration

A new EF Core migration named: AddPerformanceIndexes was created.

The migration added the following indexes:

migrationBuilder.CreateIndex(
    name: "IX_Notifications_UserId_CreatedAt",
    table: "Notifications",
    columns: new[] { "UserId", "CreatedAt" });

migrationBuilder.CreateIndex(
    name: "IX_Bookings_PhotographerId_StartTime",
    table: "Bookings",
    columns: new[] { "PhotographerId", "StartTime" });

The existing single-column `Bookings.PhotographerId` index was removed because the new composite index covers the query pattern more appropriately.

---

# 4. Performance Profiling

Performance was measured using **EF Core query logging**.

The important metric used for comparison was:

Executed DbCommand (Xms)


This represents the execution time reported for the database command.

The full HTTP request duration was not used as the main comparison because it includes other application work such as authentication, controller/service execution, serialization, and other processing.

---
# 5. Before Adding the Indexes

Before adding the indexes, EF Core logging showed:

| Query                                   |    Before |
| --------------------------------------- | --------: |
| Bookings — `PhotographerId + StartTime` | **58 ms** |
| Notifications — `UserId + CreatedAt`    | **18 ms** |

The Bookings query filters by `PhotographerId` and orders by `StartTime`, while the Notifications query filters by `UserId` and orders by `CreatedAt`.

---

# 6. After Adding the Indexes

After applying the indexes, the same queries were measured again:

| Query         |    Before | After Run 1 | After Run 2 |
| ------------- | --------: | ----------: | ----------: |
| Bookings      | **58 ms** |       66 ms |   **12 ms** |
| Notifications | **18 ms** |       38 ms |   **12 ms** |

The results varied between executions. The second runs were faster than the original measurements, but the first runs were slower, so the results should be considered **observed measurements rather than a guaranteed performance improvement**.




---

### Existing indexes should always be checked

Before creating an index, the current database schema should be inspected.

`Photographers.UserId` already had a unique index, so creating another index would have been unnecessary.

### Performance must be measured

Adding an index and assuming that the query became faster is not enough.

The query should be measured before and after the change.

In this task, EF Core's:

Executed DbCommand (Xms)


logs were used as the performance evidence.

---

The Day 4 README documents the implementation and the measured before/after results for the mentor review.
