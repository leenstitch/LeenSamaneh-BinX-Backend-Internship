Day 3 – Integration Testing

Today, I worked on integration testing for my ASP.NET Core API using xUnit and WebApplicationFactory.

I created a CustomWebApplicationFactory to run the API in a test environment.
I replaced the real SQL Server database with an In-Memory database for testing.
I added test data such as a user, patient, and vital signs.
I created a TestAuthenticationHandler to simulate an authenticated user without using a real login or JWT.
I used Claims to simulate the user's ID, email, and role.
I created integration tests for the patient API using HttpClient.
I tested the successful case when a patient exists.
I tested the NotFound case when the patient does not exist.
I also tested an authenticated endpoint using the test authentication system.

In short: I learned how to test the API through real HTTP requests while using a separate test database and test authentication.
