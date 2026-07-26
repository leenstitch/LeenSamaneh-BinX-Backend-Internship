using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Models
{
    // Order class represents a customer's order in the library system
    public class Order
    {
        public Guid Id { get; private set; }


        public Customer Customer { get; private set; }


        public List<OrderItem> Items { get; private set; }= new();


        // Constructor for Order class
        public Order(Customer customer)
        {
            Id = Guid.NewGuid();

            Customer = customer;

            customer.Orders.Add(this);
        }


        //============= METHODS ==========

        // Method to calculate the total price of the order
        public decimal CalculateTotal()
        {
            return Items.Sum(x =>
                x.Book.Price * x.Quantity);
        }


        // Method to add an item to the order
        public void AddItem(Book book, int quantity)
        {
            OrderItem item = new OrderItem(
                book,
                quantity
            );

            Items.Add(item);
        }
    }
}
