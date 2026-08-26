// ================================================================
// JwtSettings Configuration
// ================================================================
//
// This class stores the configuration settings used by the
// application's JWT authentication system.
//
// It contains the information required to:
// - Generate and validate JWT access tokens.
// - Identify the token issuer.
// - Identify the intended token audience.
// - Define access-token expiration time.
// - Define refresh-token expiration time.
//
// These settings are typically loaded from the application's
// configuration file, such as appsettings.json, using the
// Options pattern.
//
// ================================================================

namespace Cardiac_Patient_Monitoring_System.Configuration
{
    public class JwtSettings
    {
        // Secret key used to sign and validate JWT tokens.
        public string SecretKey { get; set; } = string.Empty;

        // Identifies the application or server that issues the JWT.
        public string Issuer { get; set; } = string.Empty;

        // Identifies the application or clients that are allowed
        // to use the JWT.
        public string Audience { get; set; } = string.Empty;

        // Defines how many minutes an access token remains valid.
        public int AccessTokenExpirationMinutes { get; set; }

        // Defines how many days a refresh token remains valid.
        public int RefreshTokenExpirationDays { get; set; }
    }
}