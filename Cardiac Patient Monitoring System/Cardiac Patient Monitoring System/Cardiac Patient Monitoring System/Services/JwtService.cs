// This service generates JWT access tokens for authenticated users.
// It adds user identity, roles, and permissions to the token based on
// the user's assigned roles.

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cardiac_Patient_Monitoring_System.Configuration;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        private readonly UserManager<ApplicationUser> _userManager;

        public JwtService(
            IOptions<JwtSettings> jwtSettings,
            UserManager<ApplicationUser> userManager)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
        }

        // Creates a signed JWT access token containing
        // the user's identity, roles, and permissions.
        public async Task<string> CreateToken(ApplicationUser user)
        {
            var roles =
                await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.Id.ToString()),

                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()),

                new Claim(
                    ClaimTypes.Email,
                    user.Email!)
            };

            // Adds the user's roles to the JWT claims.
            foreach (var role in roles)
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        role));
            }

            // Adds permissions assigned to Admin users.
            if (roles.Contains("Admin"))
            {
                claims.Add(
                    new Claim("Permission", "Patient.Read"));

                claims.Add(
                    new Claim("Permission", "Patient.Update"));

                claims.Add(
                    new Claim("Permission", "Patient.Delete"));

                claims.Add(
                    new Claim("Permission", "Diagnosis.Read"));

                claims.Add(
                    new Claim("Permission", "Diagnosis.Create"));

                claims.Add(
                    new Claim("Permission", "Medication.Read"));

                claims.Add(
                    new Claim("Permission", "Medication.Create"));
            }

            // Adds permissions assigned to Doctor users.
            if (roles.Contains("Doctor"))
            {
                claims.Add(
                    new Claim("Permission", "Patient.Read"));

                claims.Add(
                    new Claim("Permission", "Diagnosis.Read"));

                claims.Add(
                    new Claim("Permission", "Diagnosis.Create"));

                claims.Add(
                    new Claim("Permission", "Medication.Read"));

                claims.Add(
                    new Claim("Permission", "Medication.Create"));

                claims.Add(
                    new Claim("Permission", "VitalSign.Read"));

                claims.Add(
                    new Claim("Permission", "VitalSign.Create"));

                claims.Add(
                    new Claim("Permission", "Appointment.Read"));

                claims.Add(
                    new Claim("Permission", "Appointment.Create"));
            }

            // Adds permissions assigned to Patient users.
            if (roles.Contains("Patient"))
            {
                claims.Add(
                    new Claim("Permission", "Patient.Read"));

                claims.Add(
                    new Claim("Permission", "Patient.Update"));

                claims.Add(
                    new Claim("Permission", "VitalSign.Read"));

                claims.Add(
                    new Claim("Permission", "Medication.Read"));

                claims.Add(
                    new Claim("Permission", "Appointment.Read"));

                claims.Add(
                    new Claim("Permission", "Appointment.Create"));
            }

            // Creates the security key used to sign the JWT.
            var securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _jwtSettings.SecretKey));

            var credentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256);

            // Sets the expiration time of the access token.
            var expiration =
                DateTime.UtcNow.AddMinutes(
                    _jwtSettings.AccessTokenExpirationMinutes);

            // Creates the JWT with issuer, audience, claims,
            // expiration time, and signing credentials.
            var token =
                new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: expiration,
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}