using Microsoft.AspNetCore.Identity;

namespace Cardiac_Patient_Monitoring_System.Data
{
    public static class DbSeeder
    {
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
    }
}