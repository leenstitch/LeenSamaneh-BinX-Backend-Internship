//this service is used to register a new user , create a corresponding customer record in the database
//and handle user login by using jwt and refresh token generation.
using APIProject.Data;
using APIProject.Dto_s.Week4Dto_s.LoginDto_s;
using APIProject.Dto_s.Week4Dto_s.RegisterDto_s;
using APIProject.Interfaces.InterfacesWeek4;
using APIProject.Models;
using APIProjectWeek4Day2.Dto_s.Week4Dto_s.LoginDto_s;
using Microsoft.AspNetCore.Identity;

namespace APIProject.Services1.ServicesForWeek4
{
    public class AuthService : IAuthService
    {
        //this is used to manage user-related operations.
        private readonly UserManager<ApplicationUser> _userManager;

        // this is used to interact with the database.
        private readonly LibraryDbContext _context;

        // this is used to handle user sign-in operations.
        private readonly SignInManager<ApplicationUser> _signInManager;

        // this is used to generate JSON Web Tokens (JWTs) for authentication.
        private readonly IJwtService _jwtService;

        // this is used to manage refresh tokens for authentication.
        private readonly IRefreshTokenService _refreshTokenService;

        // Constructor to initialize the AuthService with required dependencies.
        public AuthService(
            UserManager<ApplicationUser> userManager,
            LibraryDbContext context,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService,
            IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
        }


        // Registers a new user and creates a corresponding Customer.
        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            // Start a transaction so User and Customer are created together.
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // Check if the email is already registered.
                var existingUser =
                    await _userManager.FindByEmailAsync(dto.Email);

                if (existingUser != null)
                {
                    await transaction.RollbackAsync();

                    return IdentityResult.Failed(
                        new IdentityError
                        {
                            Code = "DuplicateEmail",
                            Description = "Email is already registered."
                        });
                }

                // Create a new Identity user.
                var user = new ApplicationUser
                {
                    UserName = dto.Name,
                    Email = dto.Email
                };

                // Identity validates the password and creates the user.
                var result =
                    await _userManager.CreateAsync(user, dto.Password);

                // If Identity validation fails, return the errors.
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return result;
                }

                // Create the Customer associated with the Identity user.
                var customer = new Customer
                {
                    Name = dto.Name,
                    UserId = user.Id
                };

                _context.Customers.Add(customer);

                // Save the Customer to the database.
                await _context.SaveChangesAsync();

                // Commit only if User and Customer were both created successfully.
                await transaction.CommitAsync();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "RegistrationFailed",
                        Description = ex.Message
                    });
            }
        }

        // Logs in a user and returns a JWT and refresh token if successful.
        public async Task<TokenResponseDto?> LoginAsync(LoginDto dto)
        {
            // Find the user using the email.
            var user =
                await _userManager.FindByEmailAsync(dto.Email);

            // User does not exist.
            if (user == null)
                return null;

            // Check the submitted password.
            var result =
                await _signInManager.CheckPasswordSignInAsync(
                    user,
                    dto.Password,
                    lockoutOnFailure: false);

            // Password is incorrect.
            if (!result.Succeeded)
                return null;

            // Generate a JWT and refresh token for the authenticated user.
            var accessToken = await _jwtService.CreateToken(user);
            var refreshToken = _refreshTokenService.GenerateRefreshToken();

            // Store the refresh token in the database with an expiration date.
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(refreshTokenEntity);

            // Save the refresh token to the database.
            await _context.SaveChangesAsync();

            // Create and return the JWT.
            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}