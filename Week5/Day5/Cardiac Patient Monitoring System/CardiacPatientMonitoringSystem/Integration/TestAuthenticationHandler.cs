using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CardiacPatientMonitoringSystem.Integration
{
    // ================================================================
    // Test Authentication Handler
    // ================================================================
    //
    // This authentication handler is used only by integration tests.
    //
    // It simulates an authenticated user without requiring the real
    // JWT authentication flow.
    //
    // The test user information is taken from HTTP headers:
    //
    // X-Test-UserId
    // X-Test-Role
    //
    // If no headers are provided:
    // - UserId = 1
    // - Role = Patient
    //
    // A special X-Test-Unauthenticated header can be used to simulate
    // an unauthenticated request.
    //
    // ================================================================

    public class TestAuthenticationHandler
        : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult>
            HandleAuthenticateAsync()
        {
            // =====================================================
            // Simulate unauthenticated request
            // =====================================================

            if (Request.Headers.ContainsKey(
                    "X-Test-Unauthenticated"))
            {
                return Task.FromResult(
                    AuthenticateResult.NoResult());
            }

            // =====================================================
            // Default Test User
            // =====================================================

            var userId = "1";
            var role = "Patient";

            // =====================================================
            // Read User ID From Request Header
            // =====================================================

            if (Request.Headers.TryGetValue(
                    "X-Test-UserId",
                    out var userIdHeader))
            {
                userId =
                    userIdHeader.ToString();
            }

            // =====================================================
            // Read Role From Request Header
            // =====================================================

            if (Request.Headers.TryGetValue(
                    "X-Test-Role",
                    out var roleHeader))
            {
                role =
                    roleHeader.ToString();
            }

            // =====================================================
            // Create Claims
            // =====================================================

            var claims =
                new[]
                {
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        userId),

                    new Claim(
                        ClaimTypes.Email,
                        "test@example.com"),

                    new Claim(
                        ClaimTypes.Role,
                        role)
                };

            // =====================================================
            // Create Identity
            // =====================================================

            var identity =
                new ClaimsIdentity(
                    claims,
                    "TestAuthentication");

            // =====================================================
            // Create Principal
            // =====================================================

            var principal =
                new ClaimsPrincipal(identity);

            // =====================================================
            // Create Authentication Ticket
            // =====================================================

            var ticket =
                new AuthenticationTicket(
                    principal,
                    "TestAuthentication");

            return Task.FromResult(
                AuthenticateResult.Success(ticket));
        }
    }
}