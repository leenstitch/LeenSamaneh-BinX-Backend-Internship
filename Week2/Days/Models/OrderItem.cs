/*
    File: OrderItem.cs

    Purpose:
    This file represents individual items inside an order.

    Responsibility:
    - Connects a Book with its requested quantity.
    - Calculates the subtotal price.

    Used Files:
    - Book:
      Provides book price information.

    Concepts Applied:
    - Object composition
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Models
{
    // OrderItem class represents an item in an order, containing a Book and its quantity
    public class OrderItem
    {
        public Book Book { get; private set; }


        public int Quantity { get; private set; }
        public decimal SubTotal =>
        Book.Price * Quantity;

        // Constructor for OrderItem class
        public OrderItem(  Book book,int quantity)
        {
            Book = book;
            Quantity = quantity;
        }
    }
}
