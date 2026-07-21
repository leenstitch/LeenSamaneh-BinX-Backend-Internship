using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    // This class represents a customer with an ID, name, and a list of orders.
    public class Customer
    {
        public Guid Id { get;  private set; }
        public string Name { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();


        //============= CONSTRUCTOR ==========

        // Constructor to initialize a customer with a name and generate a unique ID.
        public Customer(string name)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name)+" can't be null.");
        }
    }
}
