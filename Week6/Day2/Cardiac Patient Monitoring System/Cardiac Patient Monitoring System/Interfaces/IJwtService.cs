// This interface defines the JWT service operation.
// It is responsible for generating an access token for an authenticated user.

using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    // Creates a JWT access token for the specified user.
    public interface IJwtService
    {
        Task<string> CreateToken(ApplicationUser user);
    }
}