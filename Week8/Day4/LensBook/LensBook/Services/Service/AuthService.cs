using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LensBook.Configuration;

using LensBook.DATA;
using LensBook.Dto_s.Auth;
using LensBook.Dto_s.RegisterCustomerDto_s;

using LensBook.Models;

using LensBook.Repository_s.IRepository;
using LensBook.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LensBook.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser>
            _userManager;

        private readonly IAuthRepository
            _authRepository;

        private readonly ApplicationDbContext
            _context;

        private readonly JwtSettings
            _jwtSettings;


        public AuthService(
            UserManager<ApplicationUser> userManager,
            IAuthRepository authRepository,
            ApplicationDbContext context,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _authRepository = authRepository;
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }


        // LOGIN
      
        public async Task<AuthResponseDto> LoginAsync(
            LoginDto dto)
        {
         
            var user =
                await _userManager.FindByEmailAsync(
                    dto.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            var passwordValid =
                await _userManager.CheckPasswordAsync(
                    user,
                    dto.Password);

            if (!passwordValid)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            var roles =
                await _userManager.GetRolesAsync(user);

            if (!roles.Any())
            {
                throw new UnauthorizedAccessException(
                    "User does not have a role.");
            }


            var accessToken =
                await GenerateAccessTokenAsync(user);



            var refreshToken =
                GenerateRefreshToken(user.Id);


            await _authRepository
                .AddRefreshTokenAsync(refreshToken);

            await _authRepository
                .SaveChangesAsync();


            return new AuthResponseDto
            {
                AccessToken = accessToken,

                RefreshToken =
                    refreshToken.Token,

                AccessTokenExpiration =
                    DateTime.UtcNow.AddMinutes(
                        _jwtSettings
                            .AccessTokenExpirationMinutes),

                RefreshTokenExpiration =
                    refreshToken.ExpiresAt
            };
        }


      
        // GENERATE ACCESS TOKEN
      
        private async Task<string>
            GenerateAccessTokenAsync(
                ApplicationUser user)
        {
       
            var roles =
                await _userManager.GetRolesAsync(user);



            var claims =
                new List<Claim>
                {
                    new Claim(
                        JwtRegisteredClaimNames.Sub,
                        user.Id.ToString()),

                    new Claim(
                        JwtRegisteredClaimNames.Email,
                        user.Email ?? string.Empty),

                    new Claim(
                        ClaimTypes.NameIdentifier,
                        user.Id.ToString())
                };


         

            foreach (var role in roles)
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        role));
            }

            if (roles.Contains("Customer"))
            {
                var customer =
                    await _authRepository
                        .GetCustomerByUserIdAsync(
                            user.Id);

                if (customer != null)
                {
                    claims.Add(
                        new Claim(
                            "CustomerId",
                            customer.CustomerId
                                .ToString()));
                }
            }


            if (roles.Contains("Photographer"))
            {
                var photographer =
                    await _authRepository
                        .GetPhotographerByUserIdAsync(
                            user.Id);

                if (photographer != null)
                {
                    claims.Add(
                        new Claim(
                            "PhotographerId",
                            photographer
                                .PhotographerId
                                .ToString()));
                }
            }

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _jwtSettings.SecretKey));


            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);



            var expiration =
                DateTime.UtcNow.AddMinutes(
                    _jwtSettings
                        .AccessTokenExpirationMinutes);


            var token =
                new JwtSecurityToken(
                    issuer:
                        _jwtSettings.Issuer,

                    audience:
                        _jwtSettings.Audience,

                    claims:
                        claims,

                    expires:
                        expiration,

                    signingCredentials:
                        credentials);



            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }


    
        // GENERATE REFRESH TOKEN
        
        private RefreshToken GenerateRefreshToken(
            int userId)
        {
            var randomBytes =
                RandomNumberGenerator.GetBytes(64);

            return new RefreshToken
            {
                Token =
                    Convert.ToBase64String(
                        randomBytes),

                UserId = userId,

                CreatedAt =
                    DateTime.UtcNow,

                ExpiresAt =
                    DateTime.UtcNow.AddDays(
                        _jwtSettings
                            .RefreshTokenExpirationDays),

                IsRevoked = false
            };
        }


      
        // REFRESH TOKEN

        public async Task<AuthResponseDto>
            RefreshTokenAsync(
                string refreshToken)
        {
          
            var storedToken =
                await _authRepository
                    .GetRefreshTokenAsync(
                        refreshToken);

            if (storedToken == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid refresh token.");
            }


            if (storedToken.IsRevoked)
            {
                throw new UnauthorizedAccessException(
                    "Refresh token has been revoked.");
            }


            if (storedToken.ExpiresAt <=
                DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException(
                    "Refresh token has expired.");
            }


            var user =
                await _userManager.FindByIdAsync(
                    storedToken.UserId.ToString());

            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    "User not found.");
            }



            var roles =
                await _userManager.GetRolesAsync(user);

            if (!roles.Any())
            {
                throw new UnauthorizedAccessException(
                    "User does not have a role.");
            }



            await _authRepository
                .RevokeRefreshTokenAsync(
                    storedToken);


          

            var newAccessToken =
                await GenerateAccessTokenAsync(user);



            var newRefreshToken =
                GenerateRefreshToken(user.Id);



            await _authRepository
                .AddRefreshTokenAsync(
                    newRefreshToken);

            await _authRepository
                .SaveChangesAsync();



            return new AuthResponseDto
            {
                AccessToken =
                    newAccessToken,

                RefreshToken =
                    newRefreshToken.Token,

                AccessTokenExpiration =
                    DateTime.UtcNow.AddMinutes(
                        _jwtSettings
                            .AccessTokenExpirationMinutes),

                RefreshTokenExpiration =
                    newRefreshToken.ExpiresAt
            };
        }


      
        // REGISTER 

        public async Task<RegisterResponseDto>
     RegisterAsync(
         RegisterCustomerDto dto)
        {
            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync();

            try
            {
              
                var user =
                    new ApplicationUser
                    {
                        UserName = dto.Email,
                        Email = dto.Email,
                        PhoneNumber = dto.PhoneNumber
                    };

                var result =
                    await _userManager.CreateAsync(
                        user,
                        dto.Password);

                if (!result.Succeeded)
                {
                    var errors =
                        string.Join(
                            ", ",
                            result.Errors.Select(
                                e => e.Description));

                    throw new UnauthorizedAccessException(
                        $"Failed to create user: {errors}");
                }


               
                var roleResult =
                    await _userManager.AddToRoleAsync(
                        user,
                        "Customer");

                if (!roleResult.Succeeded)
                {
                    var errors =
                        string.Join(
                            ", ",
                            roleResult.Errors.Select(
                                e => e.Description));

                    throw new UnauthorizedAccessException(
                        $"Failed to assign Customer role: {errors}");
                }


                var customer =
                    new Customer
                    {
                        UserId = user.Id,

                        FirstName =
                            dto.FirstName,

                        LastName =
                            dto.LastName,

                        PhoneNumber =
                            dto.PhoneNumber,

                       
                    };


                await _authRepository
                    .AddCustomerAsync(customer);


                
                await _authRepository
                    .SaveChangesAsync();


                await transaction.CommitAsync();


                
                return new RegisterResponseDto
                {
                    Message =
                        "Registration successful."
                };
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        
    }
    }
}