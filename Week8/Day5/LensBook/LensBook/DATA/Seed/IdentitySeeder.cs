using LensBook.Models;
using Microsoft.AspNetCore.Identity;

namespace LensBook.DATA.Seed
{
    public static class IdentitySeeder
    {
        // Seeds the application roles and the default StudioOwner account.
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            // ----------------------------------------------------
            // Create Roles
            // ----------------------------------------------------

            string[] roles =
            {
                "Customer",
                "Photographer",
                "StudioOwner"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole<int>(role));
                }
            }


            // ----------------------------------------------------
            // Create Studio Owner Account
            // ----------------------------------------------------

            const string ownerEmail = "owner@lensbook.com";
            const string ownerPassword = "Owner@12345";

            var owner =
                await userManager.FindByEmailAsync(ownerEmail);

            // Create the owner account if it does not exist.
            if (owner == null)
            {
                owner = new ApplicationUser
                {
                    UserName = ownerEmail,
                    Email = ownerEmail,
                    EmailConfirmed = true
                };

                var result =
                    await userManager.CreateAsync(
                        owner,
                        ownerPassword);

                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        result.Errors.Select(
                            e => e.Description));

                    throw new Exception(
                        $"Failed to create StudioOwner: {errors}");
                }
            }


            // ----------------------------------------------------
            // Assign StudioOwner Role
            // ----------------------------------------------------

            if (!await userManager.IsInRoleAsync(
                owner,
                "StudioOwner"))
            {
                await userManager.AddToRoleAsync(
                    owner,
                    "StudioOwner");
            }
        }
    }
}