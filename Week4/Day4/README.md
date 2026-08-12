Week 4 — Day 4: Input Validation with FluentValidation
Overview

Implemented input validation for the Library API using FluentValidation to enforce business rules and return clear, structured validation errors.

What Was Implemented
Installed and configured FluentValidation and ASP.NET Core integration.
Created a CreateBookValidator for validating book creation requests.
Created an UpdateBookValidator for validating book update requests.

Added business rules for:
Book title
Book price
Book quantity
Author ID

Registered validators automatically in the ASP.NET Core pipeline.
Configured automatic validation before controller actions are executed.
Tested validation behavior using Postman.
Confirmed invalid requests return 400 Bad Request with structured ValidationProblemDetails.
Created separate Postman tests for each validation rule.

Validation Examples

Field	Validation Rule :
Title	Required and maximum 100 characters
Price	Must be greater than 0
Quantity	Cannot be negative
AuthorId	Must be greater than 0

Result:

The API now automatically rejects invalid requests and returns meaningful field-specific validation messages instead of allowing invalid data to reach the controller logic.
