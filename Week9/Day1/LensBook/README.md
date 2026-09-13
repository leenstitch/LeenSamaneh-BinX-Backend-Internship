# Sprint 4 — Audit and Close Test Coverage Gaps

## 1. Sprint Planning

### Sprint Goal

Audit the capstone API test coverage, identify the highest-risk gaps, and close the most important gaps with focused automated tests.

### Sprint Backlog

* Review all capstone API endpoints and existing tests.
* Classify each endpoint by happy-path and error-path coverage.
* Identify and prioritize gaps based on risk.
* Focus first on authentication, booking/payment-adjacent, and role-protected endpoints.
* Add tests for the top 3–5 highest-priority gaps.
* Run the complete test suite and verify that all tests pass.

### Sprint 3 Retrospective Action

Improve test coverage by prioritizing critical and role-protected endpoints and making sure both successful and failure scenarios are tested.

---

## 2. Endpoint Test Coverage Audit

| Endpoint / Feature                  | Happy Path | Error Path | Coverage | Priority |
| ----------------------------------- | ---------- | ---------- | -------- | -------- |
| Register                            | ✅          | ✅          | **Both** | High     |
| Login                               | ✅          | ✅          | **Both** | High     |
| Refresh Token                       | ✅          | ✅          | **Both** | High     |
| Create Booking                      | ✅          | ✅          | **Both** | High     |
| Update Booking Status               | ✅          | ✅          | **Both** | High     |
| Add External Schedule               | ✅          | ✅          | **Both** | High     |
| Create Photographer                 | ✅          | ✅          | **Both** | High     |
| Photographer My Bookings            | ✅          | ✅          | **Both** | High     |
| Photographer My Bookings Projection | ✅          | ✅          | **Both** | High     |
| Create Session Type                 | ✅          | ✅          | **Both** | High     |
| Photographer Details + Collections  | ❌          | ❌          | **Gap**  | Medium   |
| Get Session Type By ID              | ❌          | ❌          | **Gap**  | Medium   |
| Get All Session Types               | ❌          | ❌          | **Gap**  | Medium   |
| Get My Notifications                | ❌          | ❌          | **Gap**  | Medium   |

The audit was used to identify missing coverage and determine which gaps should be addressed first.

The five highest-priority gaps identified during the audit were then covered with automated tests during Sprint 4.

---

## 3. Risk-Based Prioritization

The gaps were prioritized according to their potential impact:

1. **Authentication and authorization**
2. **Booking operations**
3. **Role-protected photographer operations**
4. **Other business and retrieval endpoints**

The highest-priority gaps selected for Sprint 4 testing were:

* Add External Schedule
* Create Photographer
* Create Session Type
* Photographer My Bookings
* Photographer My Bookings Projection

---

## 4. Tests Added to Close the Highest-Priority Gaps

### External Schedule

Added tests for:

* Valid schedule creation
* Missing user ID
* Invalid user ID
* User is not a photographer
* Invalid time range
* Overlapping schedule

### Photographer Creation

Added tests for:

* Successful photographer creation
* Existing email
* User creation failure
* Photographer role assignment failure

### Session Type Creation

Added tests for:

* Valid session type creation
* Invalid duration
* Duration exceeding the maximum limit
* Negative price

### Photographer My Bookings

Added tests for:

* Valid photographer with bookings
* Photographer not found
* Photographer with no bookings

### Photographer My Bookings Projection

Added tests for:

* Valid photographer with projected bookings
* Photographer not found
* Photographer with no bookings

These tests cover both successful scenarios and important failure or edge cases for the selected high-priority features.

---

## 5. Test Setup and Improvements

During the testing work:

* Used **Moq** to isolate repositories and verify repository interactions.
* Used **SQLite In-Memory Database** for tests requiring real EF Core queries and foreign-key relationships.
* Added the required `ApplicationUser`, `Photographer`, `Customer`, and `SessionType` records for projection tests.
* Fixed `UserManager.CreateAsync` mocking and callback setup.
* Fixed repository mock return types.
* Resolved SQLite foreign-key constraint issues in integration-style service tests.
* Verified repository interactions using `Times.Once()`.
* Covered authentication, authorization, validation, and business-rule failure scenarios where applicable.

---

## 6. Test Suite Result

The complete test suite was executed after closing the identified coverage gaps.

**Result: All tests passed successfully.**

## Tools Used

**xUnit • Moq  • Entity Framework Core • SQLite In-Memory Database**
