using Microsoft.AspNetCore.Identity;

namespace LensBook.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        // Navigation Properties

        public Customer? Customer { get; set; }

        public Photographer? Photographer { get; set; }

       
    }
}