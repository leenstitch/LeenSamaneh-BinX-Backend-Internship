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

            // Add roles to JWT
            foreach (var role in roles)
            {
                claims.Add(
                    new Claim(ClaimTypes.Role, role));
            }

            // Add permissions according to the user's role
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

            var securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _jwtSettings.SecretKey));

            var credentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256);

            var expiration =
                DateTime.UtcNow.AddMinutes(
                    _jwtSettings.AccessTokenExpirationMinutes);

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

