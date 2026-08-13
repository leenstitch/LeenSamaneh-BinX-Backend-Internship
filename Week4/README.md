Week 4 — Backend Security & Validation
Day 1 — ASP.NET Core Identity & Registration

-Added ASP.NET Core Identity to the project and connected it with Entity Framework Core and SQL Server.
-Updated LibraryDbContext to inherit from IdentityDbContext<ApplicationUser>, allowing Identity tables and user management to be handled through EF Core.
-Used ApplicationUser as the main user model for the authentication system.
-Configured Identity in Program.cs using AddIdentity<ApplicationUser, IdentityRole>().
-Implemented a Registration endpoint using UserManager to create new users and validate their registration data.
-Tested registration using Postman, including a weak password, to make sure invalid registration requests are rejected with clear errors.
-Created and applied an EF Core migration to add the Identity schema to the database.

Main concepts:
ASP.NET Core Identity — IdentityUser — IdentityRole — UserManager — IdentityDbContext — EF Core Migration

Day 2 — Login, JWT & Refresh Tokens
-Implemented a Login endpoint that verifies the user's email and password using SignInManager.
-After a successful login, the API generates a JWT Access Token containing user information such as UserId, Email, and Roles.
-Configured the JWT with an Issuer, Audience, signing key, and expiration time.
-Signed the JWT using a symmetric security key with the HMAC SHA256 algorithm.
-Implemented Refresh Tokens so users can get a new Access Token without logging in again.
-Stored Refresh Tokens in the database with an expiration date and IsRevoked status.
-Added validation to make sure a refresh token exists, has not expired, and has not been revoked.
-Implemented Refresh Token Rotation, where the old refresh token is revoked and a new one is generated after a successful refresh.

Main concepts:
JWT — Access Token — Refresh Token — Claims — SignInManager — Token Expiration — Token Rotation

Day 3 — Authorization, Roles & Protected Routes
-Protected the API endpoints using [Authorize], so unauthenticated users cannot access protected resources.
-Created different roles such as User and Admin and assigned them to test users.
-Applied Role-based Authorization to restrict specific endpoints, such as allowing only Admin users to perform certain operations.
-Created a custom authorization policy to control access based on a specific permission instead of relying only on roles.
-Tested the difference between:
-401 Unauthorized: the user is not authenticated or does not provide a valid token.
-403 Forbidden: the user is authenticated but does not have the required role or permission.
-Configured a Postman Environment to automatically store and reuse the Access Token for protected requests.

Main concepts:
[Authorize] — Roles — Admin/User — Authorization Policy — Claims — 401 — 403

Day 4 — Input Validation with FluentValidation
Added FluentValidation to the project to keep validation logic separate from DTOs and controllers.
Created a CreateBookValidator to validate book creation requests using real business rules.
Created an UpdateBookValidator to validate book update requests.
Added rules such as:
-Title cannot be empty and cannot exceed 100 characters.
Price must be greater than 0.
Quantity cannot be negative.
AuthorId must be greater than 0.
-Integrated FluentValidation into the ASP.NET Core pipeline using AddFluentValidationAutoValidation().
-This means invalid requests are validated automatically before reaching the controller action.
-Tested each validation rule individually using Postman.
-Confirmed that invalid requests return a structured 400 Bad Request response using ValidationProblemDetails.

Main concepts:
FluentValidation — AbstractValidator — RuleFor — Business Rules — 400 Bad Request — ValidationProblemDetails

Day 5 — API Security & Hardening
-Added Rate Limiting to control how many requests a client can send within a specific time period, with stricter limits for sensitive endpoints such as login.
-Configured a named CORS policy to allow requests only from specific trusted origins instead of allowing every website to access the API.
-Enabled HTTPS Redirection to ensure HTTP requests are redirected to HTTPS.
-Configured HSTS to tell browsers to always use HTTPS for the API domain.
-Reviewed the project for possible SQL Injection vulnerabilities and confirmed that EF Core LINQ queries use parameterized SQL by default.
-Reviewed raw SQL usage to make sure user input is not directly inserted into SQL queries.
-Finally, prepared a Week 4 summary in Notion covering Identity, JWT, Authorization, Validation, and API Security for the mentor check-in.

Main concepts:
Rate Limiting — CORS — HTTPS — HSTS — SQL Injection Prevention — EF Core

Week 4 Overall Summary

During Week 4, I focused on making the ASP.NET Core API secure and production-ready. 
I implemented ASP.NET Core Identity, JWT Authentication, Refresh Tokens, Role-based Authorization, Custom Authorization Policies, FluentValidation,
Rate Limiting, CORS, HTTPS, and HSTS, and reviewed the project for SQL Injection risks.
