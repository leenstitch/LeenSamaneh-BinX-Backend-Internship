// This seeder initializes the default roles and creates the
// default Admin account when it does not already exist.

using Cardiac_Patient_Monitoring_System.Models;
using Microsoft.AspNetCore.Identity;

namespace Cardiac_Patient_Monitoring_System.Data
{
    public static class DbSeeder
    {
        // Creates the default application roles if they do not exist.
        public static async Task SeedRolesAsync(
            IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<
                    RoleManager<IdentityRole<int>>>();

            string[] roles =
            {
                "Admin",

                "Patient"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole<int>
                        {
                            Name = role,
                            NormalizedName = role.ToUpper()
                        });
                }
            }
        }

        // Creates the default Admin user and assigns the Admin role.
        public static async Task SeedAdminAsync(
            IServiceProvider serviceProvider)
        {
            var userManager =
                serviceProvider.GetRequiredService<
                    UserManager<ApplicationUser>>();

            const string adminEmail = "admin@cardiac.com";
            const string adminPassword = "Admin@12345";

            var existingAdmin =
                await userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin != null)
            {
                return;
            }

            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result =
                await userManager.CreateAsync(
                    admin,
                    adminPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description));

                throw new Exception(
                    $"Failed to create admin user: {errors}");
            }

            await userManager.AddToRoleAsync(
                admin,
                "Admin");
        }
    }
}