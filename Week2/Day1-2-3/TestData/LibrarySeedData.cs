/*
    File: LibrarySeedData.cs

    Purpose:
    This file creates and stores sample data used for testing.

    Responsibility:
    - Initializes Authors, Translators, Books, Customers, and Orders.
    - Creates shared objects once using a static constructor.

    Used Files:
    - Models:
      Creates objects from Book, Customer, Order, Author, and Translator.

    Used By:
    - Day1
    - Day2
    - LibraryDataServiceForWeek2Day3

    Concepts Applied:
    - Static initialization
    - Object creation
    - Shared test data
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Models;

namespace LibrarySystem.TestData
{
    internal class LibrarySeedData
    {
        public static Admin Admin { get; }
        public static Author Author { get; }
        public static Translator Translator { get; }

        public static List<Book> Books { get; }
        public static List<Customer> Customers { get; }
        public static List<Order> Orders { get; }

        static LibrarySeedData()
        {
            Admin  = new Admin(
                "admin@gmail.com",
                "admin123",
                "Manage Books"
            );

            Author = new Author(
                "J.K Rowling",
                "Fantasy writer"
            );


            Translator = new Translator(
          "Ahmed Ali",
          "English to Arabic"
            );

            Books = new List<Book>
        {
            new Book(
                "Harry Potter",
                25,
                10,
                Author,
                Translator
            ),

            new Book(
                "1984",
                30,
                15,
                Author,
                Translator
            ),

              new Book(
                    "Animal Farm",
                    20,
                    5,
                     Author,
                Translator
                ),

                new Book(
                    "The Great Gatsby",
                    15,
                    8,
                     Author,
                Translator
                ),

                new Book(
                    "To Kill a Mockingbird",
                    20,
                    12,
                    Author,
                Translator
                ),

                new Book(
                    "Pride and Prejudice",
                    18,
                    6,
                     Author,
                Translator
                )
        };

            Customers = new List<Customer>
        {
            new Customer(
                "Leen",
                "leen@gmail.com",
                "12345",
                "customer"
            ),

                new Customer(
                    "Noor",
                    "noor@gmail.com",
                    "12345",
                    "customer"
                ),

                new Customer(
                    "Rand",
                    "rand@gmail.com",
                    "12345",
                    "customer"
                ),

                new Customer(
                    "Sara",
                    "sara@gmail.com",
                    "12345",
                    "customer"
                ),

                new Customer(
                    "Firyal",
                    "firyal@gmail.com",
                    "12345",
                    "customer"
                ),

                new Customer(
                    "Dana",
                    "dana@gmail.com",
                    "12345",
                    "customer"
                )
            };
            Orders = new List<Order>
        {
            new Order(Customers[0]),
               new Order(Customers[1]),
                new Order(Customers[2]),
                new Order(Customers[3]),
                new Order(Customers[4]),
                new Order(Customers[5]),
                new Order(Customers[3]),
                new Order(Customers[4]),
                new Order(Customers[2]),
                new Order(Customers[2]),
                new Order(Customers[1])
               
        };
         Orders[0].AddItem(Books[1], 2);
         Orders[1].AddItem(Books[0], 4);
         Orders[2].AddItem(Books[2], 1);
         Orders[3].AddItem(Books[1], 3);
         Orders[4].AddItem(Books[0], 6);
         Orders[5].AddItem(Books[1], 2);
         Orders[6].AddItem(Books[2], 1);
         Orders[7].AddItem(Books[0], 2);
         Orders[8].AddItem(Books[1], 3);
         Orders[9].AddItem(Books[2], 5);
         Orders[10].AddItem(Books[0], 2);

        }

        public static Admin GetAdmin()
        {
            return Admin;
        }
        //Adding author 
        public static Author GetAuthor()
        {
            return Author;

        }

        //Adding Translator 
        public static Translator GetTranslator()
        {
            return Translator;

        }

        //Add books
        public static List<Book> GetBooks()
            
        {
            return  Books;
        }



        //Add Customers
        public static List<Customer> GetCustomers()
        {
            return Customers;
        }



        //Get Orders
        public static List<Order> GetOrders()

        {

            return Orders;
        }
               


            


               
            
        }
    }
