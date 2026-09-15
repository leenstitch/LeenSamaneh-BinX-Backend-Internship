using System.ComponentModel.DataAnnotations;

namespace LensBook.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

      


        // Navigation Properties

        public ApplicationUser User { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
