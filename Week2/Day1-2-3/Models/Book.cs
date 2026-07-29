/*
    File: Book.cs

    Purpose:
    This file represents books in the library system.

    Responsibility:
    - Stores book information.
    - Maintains relationships with Author and Translator.

    Used Files:
    - Author:
      Represents the book author.
    - Translator:
      Represents the optional translator.

    Concepts Applied:
    - Encapsulation
    - Object relationships
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Models
{
    // Book class represents a book in the library system
    public class Book
    {
        public Guid Id { get; private set; }


        public string Title { get; private set; }


        public decimal Price { get; private set; }


        public int Quantity { get; private set; }


        public Author Author { get; private set; }


        public Translator? Translator { get; private set; }


        // Constructor for Book class
        public Book(string title,decimal price,int quantity,Author author,Translator? translator = null)
        {
            Id = Guid.NewGuid();

            Title = title;

            Price = price;

            Quantity = quantity;

            Author = author;

            Translator = translator;
        }
    }
}
