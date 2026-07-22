using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Book
    {
        // This class represents a book in the library.
        public Guid Id { get; private set; }
        private string title;
        private string author;
        private int quantity;
        private decimal price;
      


        // Properties for the book's title, author, quantity, and price.
        public string Title
        {
            get
            {
                return title;
            }
            private set
            {
                title = value;
            }
        }
        public string Author
        {
            get
            {
                return author;
            }
            private set
            {
                author = value;
            }
        }
        public int Quantity
        {
            get
            {
                return quantity;
            }
            private set
            {
                quantity = value;
            }
        }

        public decimal Price
        {
            get
            {
                return price;
            }
            private set
            {
                price = value;
            }
        }

        //============= CONSTRUCTOR ==========

        // Constructor for the Book class that initializes the book's properties and validates the input.
        public Book(string title, string author, int quantity, decimal price)
        {
            Id = Guid.NewGuid();
            this.title = title ?? throw new ArgumentNullException(nameof(title) + " cannot be null.");
            this.author = author ?? throw new ArgumentNullException(nameof(author) + "Author cannot be null.");

            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");

            else
                Quantity = quantity;


            if (price < 0)
                throw new ArgumentException("Price cannot be negative.");

            else
                Price = price;
        }


           
            
        }
    }
