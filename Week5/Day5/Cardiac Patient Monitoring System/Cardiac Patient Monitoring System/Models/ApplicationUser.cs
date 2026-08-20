using Microsoft.AspNetCore.Identity;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public Patient? Patient { get; set; }

        //public Doctor? Doctor { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }
            = new List<RefreshToken>();
    }
}
