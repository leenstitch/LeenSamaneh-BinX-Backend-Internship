// this file is used to seed the database with initial data, such as roles and users, when the application starts.
// It ensures that the necessary roles and an admin user are created if they do not already exist.
using APIProject.Models;
using Microsoft.AspNetCore.Identity;

namespace APIProject.Data
{
    public static class Seeding
    {
        // This method seeds the database with initial roles and users.
        public static async Task SeedRolesAsync(
            IServiceProvider serviceProvider)
        {
            //  Create Roles
            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Create Users
            var userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Define roles to be created
            string[] roles = { "User", "Admin" };

            // Create roles
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }

            //  Create Admin user
            var adminEmail = "admin@library.com";

            // Check if the admin user already exists
            var adminUser =
                     await userManager.FindByEmailAsync(adminEmail);

            // If the admin user does not exist, create it
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                // Create the admin user with a password
                var result = await userManager.CreateAsync(
                    adminUser,
                    "Admin123!");

                // If the creation fails, log the errors
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(
                            $"User Error: {error.Code} - {error.Description}");
                    }
                }
            }

            // Add the admin user to the "Admin" role if not already assigned
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            //  Create Normal user
            var userEmail = "user@library.com";

            // Check if the normal user already exists
            var normalUser =
              await userManager.FindByEmailAsync(userEmail);

            // If the normal user does not exist, create it
            if (normalUser == null)
            {
                normalUser = new ApplicationUser
                {
                    UserName = "User",
                    Email = userEmail,
                    EmailConfirmed = true
                };

                // Create the normal user with a password
                var result = await userManager.CreateAsync(
                    normalUser,
                    "User123!");


                // If the creation fails, log the errors
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(
                            $"User Error: {error.Code} - {error.Description}");
                    }
                }
            }

            // Add the normal user to the "User" role if not already assigned
            if (!await userManager.IsInRoleAsync(normalUser, "User"))
            {
                await userManager.AddToRoleAsync(
                    normalUser,
                    "User");
            }
        }
    }
}   