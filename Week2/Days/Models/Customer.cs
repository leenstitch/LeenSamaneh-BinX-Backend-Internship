/*
    File: Customer.cs

    Purpose:
    This file represents customers in the library system.

    Responsibility:
    - Stores customer information.
    - Maintains the customer's orders collection.

    Used Files:
    - ApplicationUser:
      Provides common user information.
    - Order:
      Represents customer's orders.

    Concepts Applied:
    - Inheritance
    - Object relationships
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Models
{
    // Customer class inherits from ApplicationUser
    public class Customer : ApplicationUser
    {
        public string Name { get; private set; }


        public List<Order> Orders { get; private set; } = new();

        // Constructor for Customer class
        public Customer( string name,string email, string password, string role)
            : base(email, password, role)// Call the base class constructor to initialize email, password, and role
        {
            Name = name;
        }
    }
}
