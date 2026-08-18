using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CardiacPatientMonitoringSystem
{
    public class TestAuthenticationHandler
        : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            Microsoft.Extensions.Logging.ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var userId = "1";

            if (Request.Headers.TryGetValue(
                "X-Test-UserId",
                out var headerValue))
            {
                userId = headerValue.ToString();
            }

            var role = "Patient";

            if (Request.Headers.TryGetValue(
                "X-Test-Role",
                out var roleHeader))
            {
                role = roleHeader.ToString();
            }

            var claims = new[]
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
            var identity = new ClaimsIdentity(
                claims,
                "TestAuthentication");

            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(
                principal,
                "TestAuthentication");

            return Task.FromResult(
                AuthenticateResult.Success(ticket));
        }
    }
}