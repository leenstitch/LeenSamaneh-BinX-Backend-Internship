// this file contains extension methods for configuring authorization policies.
using Microsoft.AspNetCore.Authorization;

namespace APIProject.Extensions
{
    public static class AuthorizationExtensions
    {
        // This extension method adds custom authorization policies to the IServiceCollection.
        public static IServiceCollection AddCustomAuthorization(
            this IServiceCollection services)
        {

            // Add authorization policies to the services collection
            services.AddAuthorization(options =>
            {

                // Define a policy named "CanManageBooks" that requires the user to have the "Admin" role.
                options.AddPolicy("CanManageBooks", policy =>
                {
                    // This policy requires the user to have the "Admin" role to access certain endpoints.
                    policy.RequireRole("Admin");
                });

                // Define a policy named "CanUpdateBook" that requires the user to have a specific claim.
                options.AddPolicy("CanUpdateBook", policy =>
                {
                    // This policy requires the user to have a claim named "Permission" with the value "UpdateBook" to access certain endpoints.
                    policy.RequireClaim("Permission", "UpdateBook");
                });
            });

            return services;
        }
    }
}