# Week 4 – Day 3

## Today's Achievements

Today, I worked on securing the API endpoints and implementing authentication and authorization features.

### Implemented

* Added `[Authorize]` to protected CRUD endpoints.
* Verified that requests without a token return `401 Unauthorized`.
* Created `User` and `Admin` roles using ASP.NET Core Identity.
* Assigned different roles to test users using `UserManager`.
* Restricted the Delete endpoint to the `Admin` role.
* Verified that a `User` token receives `403 Forbidden` when accessing the Delete endpoint.
* Created a named authorization policy based on a custom `Permission` claim.
* Added the required permission claim to the Admin JWT.
* Applied the authorization policy to the Update Book endpoint.
* Verified that:

  * User token → `403 Forbidden`
  * Admin token with the required permission → `200 OK`
* Configured Postman environment variables for:

  * `baseUrl`
  * `userAccessToken`
  * `adminAccessToken`
* Automated access-token capture after User and Admin login requests in Postman.
* Tested valid, invalid, unauthorized, and forbidden requests using Postman.

## Testing Results

| Test                              | Result             |
| --------------------------------- | -------------------|
| No token → protected endpoint     | 401 Unauthorized   |
| User token → Delete               | 403 Forbidden      |
| User token → Update               | 403 Forbidden      |
| Admin token → Delete              | 204 No Content     |
| Admin token + Permission → Update | 200 OK             |

## Technologies

* ASP.NET Core Web API
* ASP.NET Core Identity
* JWT
* Claims
* Authorization Policies
* Role-Based Authorization
* Postman
