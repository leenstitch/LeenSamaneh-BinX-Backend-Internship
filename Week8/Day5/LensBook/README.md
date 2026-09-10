# Day 5 — Sprint 3 Close-Out

## Hands-On Lab: Sprint 3 Close-Out

### 1. Demo Before/After Performance Evidence

The Sprint 3 performance results were reviewed using actual measurements collected during the sprint.

#### Query Counts

| Endpoint      | Database Queries | N+1 |
| ------------- | ---------------: | --- |
| My Bookings   |                2 | No  |
| Session Types |                1 | No  |
| Notifications |                1 | No  |

The N+1 investigation confirmed that the tested endpoints did not have an N+1 query problem.

#### Query Optimization

The `GetMyBookings` query was optimized using projection.

| Approach   | Queries | Related JOINs          | Data Retrieved       |
| ---------- | ------: | ---------------------- | -------------------- |
| Include    |       2 | Customer + SessionType | More data            |
| Projection |       2 | None                   | Only required fields |

The number of queries remained the same, while projection reduced unnecessary data retrieval.

#### Redis Cache Hit/Miss Timing

`GET /api/v1/SessionTypes` was tested with Redis caching.

| Request        | Cache State | Response Time | SQL Query |
| -------------- | ----------- | ------------: | --------- |
| First request  | Cache Miss  |       8732 ms | Yes       |
| Second request | Cache Hit   |        288 ms | No        |

The observed difference was 8444 ms.

The cache-hit request produced no `Executed DbCommand` log, confirming that the response was served from Redis without querying SQL Server.

Cache invalidation was also tested by adding a new Session Type after the cache had been populated. The cache was removed and the new data appeared in the following request.

#### Database Index Measurements

Composite indexes were added for:

* `Bookings (PhotographerId, StartTime)`
* `Notifications (UserId, CreatedAt)`

Measured results:

| Query         | Before | After Run 1 | After Run 2 |
| ------------- | -----: | ----------: | ----------: |
| Bookings      |  58 ms |       66 ms |       12 ms |
| Notifications |  18 ms |       38 ms |       12 ms |

The measurements varied between runs, so they were documented as observed results rather than guaranteed performance improvements.

---

### 2. Check Backlog Tasks Against Performance Targets

| Sprint 3 Task            | Performance Target / Expected Result                      | Result                                       |
| ------------------------ | --------------------------------------------------------- | -------------------------------------------- |
| N+1 diagnosis            | Identify unnecessary repeated queries                     | Completed — no N+1 found                     |
| Query optimization       | Reduce unnecessary data retrieval                         | Completed — projection implemented           |
| Redis caching            | Reduce database access for suitable read-heavy data       | Completed — cache hit generated no SQL query |
| Cache invalidation       | Ensure updated data appears after changes                 | Completed — invalidation tested              |
| Database indexing        | Add indexes for important filtering and ordering patterns | Completed — composite indexes added          |
| Performance measurements | Collect before/after evidence                             | Completed — measurements documented          |

Any task that does not meet its intended target should be moved to the Sprint 4 backlog rather than being counted as completed.

---

### 3. Remaining Performance Opportunities — Sprint 4 Backlog

The following performance opportunities should be tracked for Sprint 4:

* **[Performance] Stronger database benchmarking** — repeat measurements using a larger dataset and multiple runs.
* **[Performance] Query regression protection** — add automated tests for important query behavior.
* **[Performance] Cache expansion** — evaluate other read-heavy endpoints as future Redis cache candidates.

These items ensure that performance opportunities identified during Sprint 3 are not lost after the sprint ends.

---

### 4. Sprint 3 Retrospective

#### What Went Well

* EF Core query logging made database queries visible during development.
* The N+1 investigation confirmed that the tested endpoints were not suffering from N+1 queries.
* Projection reduced unnecessary data retrieval.
* Redis caching successfully served cached responses without executing SQL queries on cache hits.
* Cache invalidation was implemented and tested.
* Composite indexes were added based on actual query filtering and ordering patterns.
* Performance measurements were recorded instead of relying only on assumptions.

#### What Could Be Improved

* Database performance should be tested with a larger dataset.
* Measurements should be repeated across multiple runs to reduce the effect of caching and environmental factors.
* Execution plans should be captured when analyzing database indexes.
* Performance checks should be protected with automated regression tests.

#### One Concrete Action for Sprint 4

**Add an automated regression test for important query behavior so that future changes cannot silently reintroduce unnecessary database queries or N+1 problems.**

---

### 5. Sprint 3 Summary

The Sprint 3 summary includes:

* Before/after query measurements
* Query count and N+1 investigation results
* Query optimization using projection
* Redis caching strategy and cache hit/miss results
* Cache invalidation testing
* Database indexes and performance measurements
* Remaining Sprint 4 performance opportunities
* Sprint 3 retrospective
* Link to the merged pull request


