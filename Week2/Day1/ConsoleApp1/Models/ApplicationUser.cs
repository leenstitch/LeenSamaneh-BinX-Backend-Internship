using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Models
{
    // ApplicationUser class represents a user in the library system that can be either an Admin or a Customer
    public abstract class ApplicationUser
    {
        public Guid Id { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }
        public string Role { get; private set; }

        // Constructor for ApplicationUser class
        protected ApplicationUser(string email, string password, string role)
        {
            Id = Guid.NewGuid();

            Email = email;

            Password = password;
            Role = role;
        }
    }
}
