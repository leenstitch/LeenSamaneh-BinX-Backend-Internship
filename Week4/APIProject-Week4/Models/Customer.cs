// a Customer model class to represent the Customer entity
//this class has a relationship with the ApplicationUser class through the UserId property and the User navigation property 
//so the email and password properties are not needed in this class because they are already present in the IdentityUser class which is inherited by the ApplicationUser class
using Microsoft.AspNetCore.Identity;

namespace APIProject.Models
{
    public class Customer 
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // public string Email { get; set; } = string.Empty;// i commented this out because we are using IdentityUser which already has an Email property

        // public string Password { get; set; } = string.Empty;// i commented this out because we are using IdentityUser which already has a Password property

        public string Role { get; set; } = string.Empty;

        public string UserId { get; set; }= string.Empty;

        public ApplicationUser User { get; set; } = null;
        public List<Order> Orders { get; set; } = new();
    }
}
