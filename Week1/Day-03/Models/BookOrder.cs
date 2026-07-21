using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    // This class represents a book order, which associates a book with an order and specifies the quantity of the book in that order.
    public class BookOrder
    {
        // Properties for the book, order, and quantity.
        public Book Book { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }


        //============= CONSTRUCTOR ==========

        // Constructor for creating a new BookOrder instance.
        public BookOrder(Book book, Order order, int quantity)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book)+"can't be null");

            else
                Book = book;

            if (order == null)
                throw new ArgumentNullException(nameof(order)+"can't be null");
            else
                Order = order;


            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");
            else
               Quantity = quantity;

          
        }
    }
}
