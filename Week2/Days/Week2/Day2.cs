using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Models;
using LibrarySystem.TestData;

namespace LibrarySystem.Week2
{
    internal class Day2
    {
        public static void Week2Day2()
        {
            Author author = LibrarySeedData.GetAuthor();

            Translator translator = LibrarySeedData.GetTranslator();


            List<Book> books =
                LibrarySeedData.GetBooks();



            List<Customer> customers =
                    LibrarySeedData.GetCustomers();



            List<Order> orders =
                LibrarySeedData.GetOrders();


            Console.WriteLine("\n\n================= WEEK2 - DAY2 =================");
           
   
            //================= GROUP BY ================

            // Group orders by customer and display all orders for each customer
            var totalByCustomer = orders
               .GroupBy(o => o.Customer.Id)
               .Select(g => new
               {
                   CustomerId = g.Key,
                   Total = g.Sum(o => o.Total)
               }).ToList();


            Console.WriteLine("\nTotal amount spent by each customer by using GroupBy:");

            foreach (var item in totalByCustomer)
            {
                var customer = customers.First(c => c.Id == item.CustomerId);

                Console.WriteLine(
                    $"Customer: {customer.Name}, Total: {item.Total}"
                );
            }


            //================ JOIN =================

            //join Orders and Customers tables and display each customer and his OrderTotal
            var customerOrders = customers.Join(
                orders,
                c => c.Id,
                o => o.Customer.Id,
                (c, o) => new
                {
                    CustomerName = c.Name,
                    OrderTotal = o.Total
                }
            ).ToList();

            //Display each customer and his OrderTotal
            Console.WriteLine("\nCustomer Orders using Join:\n");
            foreach (var item in customerOrders)
            {
                Console.WriteLine(
                    $"Customer: {item.CustomerName}, Total: {item.OrderTotal}"
                );
            }



            //================ SELECT MANY ================
            // Flatten all order items from all orders into one collection

            var allOrderItems = orders
                .SelectMany(o => o.Items)
                .ToList();


            Console.WriteLine("\nAll books in all orders:\n");

            foreach (var item in allOrderItems)
            {
                Console.WriteLine(
                    $"Book: {item.Book.Title}, Quantity: {item.Quantity}"
                );
            }


            //================ DEFERRED EXECUTION ================
            // Create a query to find books with price above 20
            var expensiveBooks = books
                .Where(b => b.Price > 20);

            Console.WriteLine("\nBooks with price above 20 befor addind new books:\n");

            foreach (var book in expensiveBooks)
            {
                Console.WriteLine(
                    $"Book: {book.Title}, Price: {book.Price}"
                );
            }
            // The query is not executed yet

            Console.WriteLine("\nBooks with price above 20 after adding a new expensive book using the same query above\n");

            books.Add(new Book(
                "Clean Code",
                50,
                10,
                author,
                translator
            ));


            // Now the query executes because we enumerate it


            foreach (var book in expensiveBooks)
            {
                Console.WriteLine(
                    $"Book: {book.Title}, Price: {book.Price}"
                );
            }

        }
    }
    }

