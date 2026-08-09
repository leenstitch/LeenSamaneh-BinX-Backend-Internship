//this service is used to register a new user and create a corresponding customer record in the database.
using APIProject.Data;
using APIProject.Dto_s.RegisterDto_s;
using APIProject.Interfaces.InterfacesWeek4;
using APIProject.Models;
using Microsoft.AspNetCore.Identity;

namespace APIProject.Services1.ServicesForWeek4
{
    public class AuthService : IAuthService
{
        private readonly UserManager<ApplicationUser> _userManager;//this is used to manage user-related operations.
        private readonly LibraryDbContext _context;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            LibraryDbContext context)
        {
            _userManager = userManager;
            _context = context;
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
            catch
            {
                // If anything fails, undo all database changes.
                await transaction.RollbackAsync();

                return IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "RegistrationFailed",
                        Description =
                            "Registration failed. Please try again."
                    });
            }
        }
    }
}
