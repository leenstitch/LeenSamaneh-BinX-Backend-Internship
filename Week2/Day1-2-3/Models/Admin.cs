/*
    File: Admin.cs

    Purpose:
    This file represents an administrator user in the library system.

    Responsibility:
    - Extends ApplicationUser with admin-specific behavior.

    Used Files:
    - ApplicationUser:
      Parent class that provides common user properties.

    Concepts Applied:
    - Inheritance
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Models
{
    // Admin class inherits from ApplicationUser
    public class Admin : ApplicationUser
    {
        


        public Admin(string email,string password, string role)

        : base(email, password, role) // Call the base class constructor to initialize email, password, and role
        {
                
        }
    }
}
