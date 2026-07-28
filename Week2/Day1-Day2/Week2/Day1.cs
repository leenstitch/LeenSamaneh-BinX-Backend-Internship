/*
    WEEK2 DAY1 - Generics & Advanced Collections

    Files used:

    - Models:
      Contains the library entities:
      Book, Customer, Order, Author, Translator.

    - TestData:
      Contains LibrarySeedData which provides sample data
      used for testing repositories.

    - Interfaces:
      Contains IRepository<T> which defines the generic repository rules.

    - Repositories:
      Contains Repository<T> implementation.
      It provides reusable operations for different entity types.

    - Week2 Day1:
      Tests the generic repository by using it with
      different models such as Book and Customer.

    Concepts demonstrated:
    - Generic classes
    - Generic constraints (where T : class)
    - IReadOnlyList
    - Predicate based searching
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Models;
using LibrarySystem.Repositories;
using LibrarySystem.TestData;

namespace LibrarySystem.Week2
{
    internal class Day1
    {
        public static void Week2Day1()
        {


            Console.WriteLine("================= WEEK2 - DAY1 =================\n\n");

            Admin admin = LibrarySeedData.GetAdmin();
            Author author = LibrarySeedData.GetAuthor();
            Translator translator = LibrarySeedData.GetTranslator();
            List<Book> books = LibrarySeedData.GetBooks();
            List<Customer> customers =LibrarySeedData.GetCustomers();



            // ===== Test Author =====
            // Display the author's information
            Console.WriteLine("bring author information");
            Console.WriteLine("\nAuthor:");
            Console.WriteLine($"author name: {author.Name}");
            Console.WriteLine($"author Biography: {author.AuthorBiography}");



            // ===== Test Translator =====
            // Display the translator's information
            Console.WriteLine("\nbring translator information");
            Console.WriteLine("\nTranslator:");
            Console.WriteLine($"translator name:  {translator.Name}");
            Console.WriteLine($"translator Language: {translator.Language}");



            // ===== Test Book =====

            // Display the book's information
            Console.WriteLine("\nbring book information");
            Console.WriteLine("\nBook:");
            Console.WriteLine($"book title: {books[0].Title}");
            Console.WriteLine($"book price: {books[0].Price}");
            Console.WriteLine($"book quantity: {books[0].Quantity}");

            Console.WriteLine(
                $"Author: {books[0].Author.Name}"
            );

            Console.WriteLine(
                $"Translator: {books[0].Translator.Name}"
            );



            // ===== Test Customer =====
          
            // Display the customer's information
            Console.WriteLine("\nbring customer information");
            Console.WriteLine("\nCustomer:");
            Console.WriteLine($"customer name: {customers[0].Name}");
            Console.WriteLine($"customer email: {customers[0].Email}");
            Console.WriteLine($"customer password: {customers[0].Password} (the password is secret but displayed for testing purposes)");
            Console.WriteLine($"customer role: {customers[0].Role}");



            // ===== Test Admin =====
           
            // Display the admin's information
            Console.WriteLine("\nbring admin information");
            Console.WriteLine("\nAdmin:");
            Console.WriteLine($"admin email: {admin.Email}");
            Console.WriteLine($"admin password: {admin.Password} (the password is secret but displayed for testing purposes)");
            Console.WriteLine($"admin role: {admin.Role}");




            // ===== Test Order =====
            Order order = new Order(customers[0]);

            Console.WriteLine("\nadd order item");
            order.AddItem(books[0], 2);// Add 2 copies of the book to the order


            // Display the order's information
            Console.WriteLine("\nbring order information");
            Console.WriteLine("\nOrder:");
            Console.WriteLine($"order ID: {order.Id}");

            Console.WriteLine(
                $"Customer Name: {order.Customer.Name}"
            );


            Console.WriteLine(
                $"Total Cost: {order.CalculateTotal()}"
            );


            // Create a generic repository for Book objects.
            // Repository<T> can work with any reference type because of the where T : class constraint.
            Repository<Book> bookRepository = new Repository<Book>();


            // Create another repository using a different model type.
            // This demonstrates that the same generic repository can be reused with multiple entities.
            Repository<Customer> customerRepository = new Repository<Customer>();



            // Add all books from the seed data into the Book repository.
            // The repository stores Book objects internally using List<T>.
            foreach (var bookss in LibrarySeedData.Books)
            {
                bookRepository.Add(bookss);
            }


            // Add all customers from the seed data into the Customer repository.
            // The same Add method works with Customer objects because Repository<T> is generic.
            foreach (var customer in LibrarySeedData.Customers)
            {
                customerRepository.Add(customer);
            }

            // GetAll returns IReadOnlyList<T>.
            // The returned collection can be read but cannot be modified directly.
            // This confirms the use of IReadOnlyList in the generic repository.
            Console.WriteLine($"# of Books: {bookRepository.GetAll().Count}");

            Console.WriteLine($"# of Customers: {customerRepository.GetAll().Count}");


            // Test the Find method using a predicate.
            // The predicate searches for a book where the Title matches "Harry Potter".
            var book = bookRepository.Find(
                b => b.Title == "Harry Potter"
            );

            // Display the found book.
            // The ?. operator prevents errors if no matching book is found.
            Console.WriteLine($"Found Book: {book?.Title}");
        }
    }
}