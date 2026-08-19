using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IJwtService
    {
        Task<string> CreateToken(ApplicationUser user);
    }
}
