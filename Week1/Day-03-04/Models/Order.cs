using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Dtos;
using Library.Interfaces;

namespace Library.Models
{
    // This class represents an order made by a customer, containing details about the customer and the books ordered.
    public class Order
    {
       public Guid Id { get;private set; }
        public Customer Customer { get;private set; }
        public List<BookOrder> BookOrders { get; private set; } = new List<BookOrder>();


        
        // ============ CONSTRUCTOR ==========
        
        // Constructor to initialize an order with a customer and generate a unique ID.
        public Order(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer)+ " is null.");

            Id = Guid.NewGuid();
            Customer = customer;
            customer.Orders.Add(this);

        }

        //============= METHODS =============

        // Method to add a book to the order with a specified quantity.
        public void AddBook(Book book, int quantity)
        {
            BookOrder item = new BookOrder(
                       book,
                       this,
                       quantity
                   );

            BookOrders.Add(item);
        }

        // Method to calculate the total cost of the order by summing the price of each book multiplied by its quantity.
        public decimal CalculateTotal()
        {
            decimal total = 0;

            foreach (var item in BookOrders)
            {
                total += item.Book.Price * item.Quantity;
            }

            return total;
        }

        // Method to process the payment for the order using a specified payment method.
        public PaymentResult ProcessPayment(IPayment payment)

        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment)+ "can't be null");

            decimal total = CalculateTotal();

            return payment.Pay(total);
        }
    }
}
