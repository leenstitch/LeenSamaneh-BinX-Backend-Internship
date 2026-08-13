//this model class inherits from IdentityUser and adds a navigation property for the person entitys such as Customer
using Microsoft.AspNetCore.Identity;

namespace APIProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Customer? Customer { get; set; }
    }
}
