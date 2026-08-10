//this file contains the implementation of the jwt service that generates JWT tokens.
/* 
  related files:
  interface: IJwtService.cs
*/ 

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APIProject.Interfaces.InterfacesWeek4;
using APIProject.Models;
using Microsoft.IdentityModel.Tokens;

namespace APIProject.Services1.ServicesForWeek4
{
    public class JwtService : IJwtService
    {
        // The configuration object used to access JWT settings from the appsettings.json file.
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method creates a JWT token for the specified user.
        public string CreateToken(ApplicationUser user)
        {

            // Retrieve JWT settings from the configuration.
            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expiryMinutes =
                int.Parse(_configuration["Jwt:ExpiryMinutes"]!);

            // Define the claims to be included in the JWT token.
            // Claims are pieces of information about the user that are encoded in the token.
            var claims = new[]
            {
                // The subject claim (sub) represents the unique identifier of the user.
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            // Create a symmetric security key using the secret key from the configuration.
            var securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key!));

            // Create signing credentials using the security key and the HMAC SHA256 algorithm.
            var credentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256);

            // Set the expiration time for the token based on the configured expiry minutes.
            var expiration = DateTime.UtcNow.AddMinutes(expiryMinutes);
    
            // Create the JWT token.
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            // Serialize the token to a string and return it.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    }
