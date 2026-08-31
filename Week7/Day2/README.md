# Week 7 — Day 2: Authentication Endpoints & Testing

## Overview

Today focused on implementing and testing the authentication endpoints for the LensBook backend.

## Tasks Completed

### 1. Authentication Endpoints

Implemented authentication endpoints for:

* Customer Registration
* User Login
* Refresh Token

The endpoints support authentication for the different user roles in the system, including:

* Customer
* Photographer
* StudioOwner

### 2. JWT Authentication

Implemented the login flow to generate authentication tokens containing the authenticated user's information and role.

The user's role is returned through the JWT claims so the system can identify whether the authenticated user is a Customer, Photographer, or StudioOwner.

### 3. Postman Testing

Tested the authentication endpoints using Postman.

The following flows were tested:

* Customer registration
* Customer login
* Access Token generation
* Refresh Token generation
* Role information in the JWT
* Refresh Token request

### 4. API Verification

Verified the authentication endpoints through Postman and checked the returned responses and generated tokens.

## Tools Used

* ASP.NET Core
* JWT
* ASP.NET Core Identity
* Postman

## Result

Successfully implemented and tested the authentication endpoints and verified the authentication flow using Postman.
