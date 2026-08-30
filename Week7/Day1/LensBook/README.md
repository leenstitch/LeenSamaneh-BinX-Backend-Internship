# Week 7 — Sprint 2: Identity, Authentication & Authorization

## Project: LensBook

LensBook is a photography studio booking platform designed to manage photography sessions, customers, photographers, studio bookings, and photographers' external schedules.

The project is being developed as a new capstone project to apply the concepts required during Sprint 2, especially **ASP.NET Core Identity, authentication, authorization, role management, database migrations, testing, and secure API design**.

---

# Day 1 — Sprint 2 Planning & Identity Integration

## 1. Sprint 2 Goal

The main goal of Sprint 2 is to build a secure authentication and authorization system for the LensBook application using **ASP.NET Core Identity**.

The sprint focuses on:

* Integrating ASP.NET Core Identity into the existing application.
* Creating and managing application users.
* Defining the roles required by the LensBook domain.
* Planning role-based access to API endpoints.
* Preparing the project for authentication and authorization.
* Applying Identity database migrations without affecting existing domain data.
* Preparing a testing strategy for the new authentication and authorization functionality.

### Sprint Goal Statement

> **Build a secure authentication and authorization foundation for LensBook using ASP.NET Core Identity, with clearly defined domain roles and role-based access rules for the API.**

---

# 2. Sprint Backlog

The Sprint 2 backlog is organized around authentication, authorization, domain functionality, and testing.

| #  | Backlog Task           | Description                                                                           | Status  |
| -- | ---------------------- | ------------------------------------------------------------------------------------- | ------- |
| 1  | Sprint Planning        | Define Sprint 2 goal, roles, backlog, and authorization strategy.                     | Done    |
| 2  | Create ApplicationUser | Create a custom Identity user based on `IdentityUser<int>`.                           | Done    |
| 3  | Integrate Identity     | Change the application DbContext to inherit from `IdentityDbContext`.                 | Done    |
| 4  | Configure Database     | Connect LensBook to a local SQL Server database.                                      | Done    |
| 5  | Create Migration       | Generate and review the initial database migration.                                   | Done    |
| 6  | Apply Migration        | Apply the migration to the local database.                                            | Done    |
| 7  | Define Roles           | Define Customer, Photographer, and StudioOwner roles.                                 | Done    |
| 8  | Seed StudioOwner       | Create the StudioOwner role and seed the StudioOwner account.                         | Done    |
| 9  | Authentication         | Implement user registration and login using Identity/JWT.                             | Planned |
| 10 | Authorization          | Protect endpoints according to user roles.                                            | Planned |
| 11 | Customer Features      | Implement customer booking and booking management functionality.                      | Planned |
| 12 | Photographer Features  | Implement photographer schedule and booking management functionality.                 | Planned |
| 13 | StudioOwner Features   | Implement studio-level management functionality.                                      | Planned |
| 14 | External Schedules     | Allow photographers to record their external photography schedules.                   | Planned |
| 15 | Testing                | Add unit and integration tests for authentication, authorization, and core endpoints. | Planned |

---

# 3. Sprint 1 Retrospective Action

One of the main improvements carried forward from the previous sprint is to follow a more structured development and testing process.

Instead of implementing functionality first and testing afterward, Sprint 2 will follow a more deliberate order:

1. Define the requirement.
2. Define the role and authorization rules.
3. Design the endpoint.
4. Implement the service and repository logic.
5. Write the required tests.
6. Verify the endpoint using Swagger/Postman.

This approach helps keep authorization consistent across the entire API instead of adding `[Authorize]` attributes randomly after endpoints have already been implemented.

---

# 4. LensBook Domain

LensBook focuses on photography studio bookings.

The system has three main user roles:

* **Customer**
* **Photographer**
* **StudioOwner**

Customers can book photography sessions inside the studio.

Photographers can view their studio bookings and manage their own availability and external schedules.

The StudioOwner manages the studio and has access to administrative functionality.

---

# 5. Domain Entities

The current LensBook domain contains the following entities:

| Entity             | Purpose                                                                 |
| ------------------ | ----------------------------------------------------------------------- |
| `Customer`         | Stores customer-specific information.                                   |
| `Photographer`     | Stores photographer-specific information.                               |
| `Booking`          | Represents a photography session booked inside the studio.              |
| `SessionType`      | Defines the type and duration of a photography session.                 |
| `ExternalSchedule` | Stores a photographer's external photography appointments or schedules. |
| `Reminder`         | Stores reminders related to customers or bookings.                      |
| `ReminderType`     | Defines the type/category of a reminder.                                |

In addition to these domain entities, the application uses:

ApplicationUser

for authentication and Identity functionality.

---

# 6. ApplicationUser

LensBook uses a custom `ApplicationUser` class:

public class ApplicationUser : IdentityUser<int>
{
    public Customer? Customer { get; set; }

    public Photographer? Photographer { get; set; }
}


`ApplicationUser` extends:

IdentityUser<int>

This allows the application to use ASP.NET Core Identity while still allowing additional domain-specific information to be stored in the `Customer` and `Photographer` entities.

The relationships are:


ApplicationUser
      │
      ├── Customer
      │
      └── Photographer


The StudioOwner does not require a separate domain entity because the StudioOwner currently only needs Identity information and a role.

---

# 7. Identity Integration

The existing application DbContext was configured to use ASP.NET Core Identity.

The DbContext now inherits from:


IdentityDbContext<ApplicationUser, IdentityRole<int>, int>


This enables Identity tables such as:

* `AspNetUsers`
* `AspNetRoles`
* `AspNetUserRoles`
* `AspNetUserClaims`
* `AspNetUserLogins`
* `AspNetUserTokens`
* `AspNetRoleClaims`

to exist alongside the LensBook domain tables.

The domain entities remain part of the same `ApplicationDbContext`.

---

# 8. Database Configuration

LensBook uses a local SQL Server database during development.

The configured database is:

LensBookDb


The application uses Entity Framework Core with SQL Server.

The connection string is configured in:

appsettings.json

The database context is registered using:

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

---

# 9. Entity Framework Core Migration

Because LensBook is a new project, the initial migration was created after configuring the domain entities and Identity.

Migration:

InitialCreate


The migration creates the required LensBook and Identity database tables.

The migration was applied successfully to the local SQL Server database using:

Update-Database


This confirms that the application can successfully create and use its database schema.

---

# 10. Roles

LensBook currently uses three roles:

| Role           | Description                                                  |
| -------------- | ------------------------------------------------------------ |
| `Customer`     | A customer who books photography sessions.                   |
| `Photographer` | A photographer who manages their own bookings and schedules. |
| `StudioOwner`  | The owner/administrator of the photography studio.           |

The roles are intentionally defined at the beginning of Sprint 2 so that authorization rules can be applied consistently across the application.

---

# 11. Role Responsibilities

## Customer

The Customer will be able to:

* Register an account.
* Log in.
* View available photography session types.
* Create a studio booking.
* View their own bookings.
* View details of their own bookings.
* Cancel their own bookings.
* Manage their own profile.

A customer must not be able to:

* View another customer's bookings.
* Manage another photographer's schedules.
* Access StudioOwner functionality.

---

## Photographer

The Photographer will be able to:

* Log in.
* View their own studio bookings.
* View booking details assigned to them.
* Cancel bookings assigned to them when allowed.
* Manage their own external schedules.
* Add external photography appointments.
* View their own external schedules.
* Manage their own availability.

A photographer must not be able to:

* View another photographer's private schedules.
* Manage StudioOwner functionality.
* Access another customer's private information beyond what is required for their booking.

---

## StudioOwner

The StudioOwner will be able to:

* Log in.
* Manage studio-level data.
* Manage session types.
* View bookings.
* Manage photographers.
* View customer-related booking information.
* Access administrative endpoints.

The StudioOwner role is implemented through ASP.NET Core Identity and does not currently require a separate `StudioOwner` entity.

---

# 12. Authorization Plan

The authorization strategy will use ASP.NET Core role-based authorization.

Example:

[Authorize(Roles = "Customer")]


or:


[Authorize(Roles = "Photographer")]


or:

[Authorize(Roles = "StudioOwner")]


Some endpoints may be available to multiple roles:


[Authorize(Roles = "Customer,Photographer")]


The exact authorization rules will be implemented when the corresponding endpoints are created.

---

# 13. Planned Endpoint Authorization

| Endpoint Function                | Customer | Photographer | StudioOwner |
| -------------------------------- | :------: | :----------: | :---------: |
| Register                         |  Public  |    Public    |      No     |
| Login                            |  Public  |    Public    |    Public   |
| View Session Types               |     ✓    |       ✓      |      ✓     |
| Create Booking                   |     ✓    |       ✓      |      ✓     |
| View Own Bookings                |     ✓    |       ✓      |      ✓     |
| View Photographer's Own Bookings |    No    |       ✓      |      ✓     |
| Cancel Own Booking               |     ✓    |       ✓      |      ✓     |
| Manage External Schedule         |    No    |       ✓      |      ✓     |
| View Own External Schedule       |    No    |       ✓      |      ✓     |
| Manage Session Types             |    No    |      No      |      ✓     |
| Manage Photographers             |    No    |      No      |      ✓     |
| Manage Customers                 |    No    |      No      |      ✓     |
| Studio Administration            |    No    |      No      |      ✓     |

Ownership checks will also be applied where necessary.

For example, being authenticated as a Photographer is not enough to access every photographer's bookings. The service layer must verify that the requested booking belongs to the authenticated photographer.


# 14. Booking Concept

A `Booking` represents a photography session inside the studio.

A booking contains:

* Customer
* Photographer
* Session Type
* Start Time
* End Time
* Status
* Notes
* Created At
* Updated At

The `SessionType` determines the duration of the photography session.

Examples include:

* Graduation session — 1 hour
* Birthday session — 1 hour
* Wedding session — 2 hours
* Standard photography session — 30 minutes

The booking system will use the session duration when determining the booking's end time.

---

# 15. External Schedule Concept

External photography appointments are different from studio bookings.

A photographer may have an external photography appointment such as:

* Wedding photography outside the studio.
* Outdoor photography.
* Photography in another city.
* Private photography work.

These schedules are **not customer bookings through the LensBook studio booking system**.

Instead, the photographer records their external schedule so that their availability can be managed correctly.

The external schedule may contain information such as:

* Date
* Start time
* End time
* Location
* Description
* Notes

This separation prevents external photography work from being treated as a normal studio booking.

---

# 16. Reminder Concept

LensBook will also support reminders.

A `Reminder` is associated with a patient/customer and a `ReminderType`.

The `ReminderType` entity defines the category of the reminder.

Examples could include:

* Upcoming booking
* Payment reminder
* Session reminder
* Follow-up reminder

Reminder functionality will be implemented in a later task.

---

# 17. Identity Seeding

Identity roles are stored in the database through ASP.NET Core Identity.

The project also contains an `IdentitySeeder` responsible for creating the initial StudioOwner account.

The StudioOwner account is seeded with:


Role: StudioOwner


The seeding process ensures that the initial StudioOwner account and role exist when the application starts.

The seeder checks whether the role or user already exists before creating them, preventing duplicate records.

---

# 18. Testing Plan

Testing is part of Sprint 2 and will be applied throughout development.

The testing strategy will include:

### Unit Testing

Unit tests will be used for:

* Services
* Business rules
* Ownership checks
* Booking logic
* Authorization-related service behavior

The project will use:


xUnit
Moq


for unit testing.

The standard testing structure will follow:


Arrange
Act
Assert


---

### Integration Testing

Integration tests will verify the behavior of the API as a whole.

The project will use:


WebApplicationFactory


to test API endpoints.

Integration tests will cover scenarios such as:

* Successful authentication.
* Unauthorized requests.
* Forbidden requests.
* Customer access.
* Photographer access.
* StudioOwner access.
* Booking creation.
* Booking cancellation.
* Ownership restrictions.

---

# 19. Security Testing Scenarios

The following authorization scenarios will be tested:

### Customer


Customer → Create own booking → Allowed
Customer → View own booking → Allowed
Customer → View another customer's booking → Denied
Customer → Manage external photographer schedule → Denied
Customer → Access StudioOwner endpoint → Denied


### Photographer


Photographer → View own bookings → Allowed
Photographer → Cancel own booking → Allowed
Photographer → Manage own external schedule → Allowed
Photographer → Manage another photographer's schedule → Denied
Photographer → Access StudioOwner endpoint → Denied


### StudioOwner


StudioOwner → View bookings → Allowed
StudioOwner → Manage session types → Allowed
StudioOwner → Manage photographers → Allowed
StudioOwner → Access administrative endpoints → Allowed

---

# 20. Current Day 1 Progress

The following Day 1 tasks have been completed:

* Sprint 2 goal defined.
* Sprint backlog created.
* LensBook domain roles identified.
* `ApplicationUser` created.
* ASP.NET Core Identity integrated.
* `ApplicationDbContext` configured with `IdentityDbContext`.
* Local SQL Server database configured.
* Initial EF Core migration created.
* Database migration successfully applied.
* `Customer` entity created.
* `Photographer` entity created.
* `Booking` entity created.
* `SessionType` entity created.
* `ExternalSchedule` entity created.
* `Reminder` entity created.
* `ReminderType` entity created.
* Identity roles defined.
* StudioOwner seeding implemented.
* Initial authorization strategy documented.
* Testing strategy planned.

---

# 21. Remaining Sprint 2 Work

The main remaining work includes:

1. Implement user registration.
2. Implement login.
3. Implement JWT authentication.
4. Configure authentication middleware.
5. Apply `[Authorize]` to protected endpoints.
6. Implement role-based authorization.
7. Implement Customer booking functionality.
8. Implement Photographer booking functionality.
9. Implement Photographer external schedules.
10. Implement StudioOwner management functionality.
11. Implement Reminder functionality.
12. Add unit tests.
13. Add integration tests.
14. Test unauthorized and forbidden access.
15. Test ownership rules.
16. Verify the complete API through Swagger and Postman.

---

# 22. Day 1 Deliverables

By the end of Day 1, LensBook has a clear Sprint 2 direction and an Identity foundation.

The completed deliverables are:

* Defined Sprint 2 goal.
* Defined Sprint backlog.
* Defined domain roles.
* Integrated ASP.NET Core Identity.
* Created custom `ApplicationUser`.
* Integrated Identity with the existing `ApplicationDbContext`.
* Configured local SQL Server.
* Created and applied EF Core migration.
* Seeded the initial StudioOwner account.
* Documented role responsibilities.
* Documented planned endpoint authorization.
* Defined the testing strategy.

The project is now ready for the implementation of authentication, authorization, and the main LensBook API functionality in the following Sprint 2 tasks.
