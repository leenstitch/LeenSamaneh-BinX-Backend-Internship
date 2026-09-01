# LensBook – Day 3

## Overview

Day 3 focused on implementing **Authentication, Authorization, Role-Based Access Control (RBAC), and Ownership Checks** in the LensBook photography booking API.

The goal was to secure API endpoints and make sure that users can only access the resources they are authorized to access.

---

## Work Completed

### 1. Role-Based Access Control (RBAC)

Applied authorization rules to API endpoints based on user roles.

Examples:

[Authorize(Roles = "StudioOwner")]
Only users with the StudioOwner role can create photographers.

Photographer-specific endpoints use:

[Authorize(Roles = "Photographer")]
This prevents customers or other roles from accessing photographer-only functionality.


[Authorize(Roles = "customers")]
Only users with the customers role can book session.

---

### 2. Ownership Checks

Implemented ownership validation for photographer bookings.

The photographer does not send their PhotographerId manually.

Instead, the system gets the authenticated user's ID from the JWT:

---

### 3. Photographer Bookings

Added functionality for photographers to retrieve their own bookings.

Endpoint:

GET /api/v1/PhotographersController/my-bookings


The endpoint:

1. Requires the Photographer role.
2. Gets the authenticated user's ID from the JWT.
3. Finds the related photographer.
4. Retrieves bookings belonging to that photographer.
5. Returns the booking information.

---

### 4. Booking Management

Implemented booking functionality connecting:

* Customer
* Photographer
* Session Type
---

### 5. Session Types

Implemented session type management.

Session types represent the photography services available for customers to choose from.

---

### 6. Photographer Management

Implemented photographer creation and management.

A photographer is connected to an Identity user.


Photographers are created through a protected endpoint available to the StudioOwner.

---

### 7. Authorization Testing

Tested the authorization behavior using Postman.

The tests verify that:

* Customers cannot access StudioOwner endpoints.
* Unauthorized roles receive 403 Forbidden.
* Photographers can access photographer-specific endpoints.
* A photographer cannot access another photographer's bookings.
* Authenticated users receive access only to resources they own.
<img width="407" height="735" alt="image" src="https://github.com/user-attachments/assets/b63f9770-892c-4322-abf0-e2376287acbc" />

---

## Day 3 Outcome

By the end of Day 3, the LensBook API has a secured authentication and authorization structure with:
<img width="383" height="390" alt="image" src="https://github.com/user-attachments/assets/73613bf2-ace4-4602-a4cf-d0d0794a1602" />

* JWT authentication.
* Identity roles.
* Role-based endpoint protection.
* Ownership checks.
* Secure photographer booking access.
* Customer, Photographer, and StudioOwner roles.
* Repository and Service architecture.
* EF Core database migrations.
* Postman authorization testing.

The main focus was ensuring that **being authenticated is not enough by itself — the user's role and ownership of the requested resource are also verified.**
