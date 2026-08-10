# Week 4 — Day 2: JWT Authentication & Refresh Tokens

## Overview

Today, I implemented and tested an authentication system using **ASP.NET Core Identity, JWT Access Tokens, and Refresh Tokens**.

## What I Worked On

* Implemented **User Registration** using ASP.NET Core Identity.
* Implemented **User Login** and password validation.
* Created a **JWT Access Token** containing user information such as ID and email.
* Configured JWT settings including:

  * Key
  * Issuer
  * Audience
  * Expiration time
* Configured JWT authentication and token validation in `Program.cs`.
* Protected API endpoints using `[Authorize]`.
* Tested authenticated requests using **Postman**.
* Tested **Access Token expiration** and verified that expired tokens return `401 Unauthorized`.
* Implemented **Refresh Tokens** with a longer lifetime of 7 days.
* Created a `RefreshToken` model and stored refresh tokens in the database.
* Created `TokenResponseDto` to return both Access and Refresh Tokens after login.
* Implemented the **Refresh Token flow** to generate a new Access Token after the old one expires.
* Tested the complete authentication and refresh process using Postman.
* Added Postman collection and environment files for testing.

## What I Learned

* Difference between **Authentication** and **Authorization**.
* How JWT Access Tokens are created, signed, validated, and expired.
* How `[Authorize]` protects API endpoints.
* How Access Tokens are used for normal API requests.
* Why Refresh Tokens are used to obtain new Access Tokens.
* How Access Tokens and Refresh Tokens have different lifetimes and purposes.
* How the frontend can use the Refresh Token to obtain a new Access Token without requiring the user to log in again.
* How to test authentication, expired tokens, and refresh flows using Postman.

## Authentication Flow

```text
Login
  ↓
Access Token + Refresh Token
  ↓
Use Access Token for API requests
  ↓
Access Token expires
  ↓
Use Refresh Token
  ↓
New Access Token
  ↓
Continue using the API
```

## Testing

I tested the following cases using Postman:

* Successful registration → `200 OK`
* Successful login → Access Token + Refresh Token
* Valid Access Token → `200 OK`
* Expired Access Token → `401 Unauthorized`
* Refresh Token → New Access Token
* New Access Token → `200 OK`
* Expired/invalid token → `401 Unauthorized`
