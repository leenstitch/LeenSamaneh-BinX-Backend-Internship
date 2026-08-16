//this file is used to define the IJwtservice interface .
using APIProject.Models;

namespace APIProject.Interfaces.InterfacesWeek4
{
    public interface IJwtService
    {
        // This interface defines a contract for generating JSON Web Tokens (JWT) for application users.
        Task<string> CreateToken(ApplicationUser user);
    }
}
