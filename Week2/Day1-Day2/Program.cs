using LibrarySystem.Models;


namespace LibrarySystem
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Library System Test");


            // ===== Test Author =====
            Console.WriteLine("\nTesting Author class(add author)");
            // Create an instance of the Author class
            Author author = new Author(
                "J.K Rowling",
                "Fantasy writer"
            );

            // Display the author's information
            Console.WriteLine("bring author information");
            Console.WriteLine("\nAuthor:");
            Console.WriteLine($"author name: {author.Name}");
            Console.WriteLine($"author Biography: {author.AuthorBiography}");



            // ===== Test Translator =====
            Console.WriteLine("\nTesting Translator class(add translator)");
            // Create an instance of the Translator class
            Translator translator = new Translator(
                "Ahmed Ali",
                "English to Arabic"
            );
            // Display the translator's information
            Console.WriteLine("bring translator information");
            Console.WriteLine("\nTranslator:");
            Console.WriteLine($"translator name:  {translator.Name}");
            Console.WriteLine($"translator Language: {translator.Language}");



            // ===== Test Book =====
            Console.WriteLine("\nTesting Book class(add book)");
            // Create an instance of the Book class
            Book book1 = new Book(
                "Harry Potter",
                25,
                10,
                author,
                translator
            );

            // Display the book's information
            Console.WriteLine("bring book information");
            Console.WriteLine("\nBook:");
            Console.WriteLine($"book title: {book1.Title}");
            Console.WriteLine($"book price: {book1.Price}");
            Console.WriteLine($"book quantity: {book1.Quantity}");

            Console.WriteLine(
                $"Author: {book1.Author.Name}"
            );

            Console.WriteLine(
                $"Translator: {book1.Translator.Name}"
            );



            // ===== Test Customer =====
            Console.WriteLine("\nTesting Customer class(add customer)");
            // Create an instance of the Customer class
            Customer customer1 = new Customer(
                "Leen",
                "leen@gmail.com",
                "12345",
                "customer"
            );
            // Display the customer's information
            Console.WriteLine("bring customer information");
            Console.WriteLine("\nCustomer:");
            Console.WriteLine($"customer name: {customer1.Name}");
            Console.WriteLine($"customer email: {customer1.Email}");
            Console.WriteLine($"customer password: {customer1.Password} (the password is secret but displayed for testing purposes)");
            Console.WriteLine($"customer role: {customer1.Role}");



            // ===== Test Admin =====
            Console.WriteLine("\nTesting Admin class(add admin)");
            // Create an instance of the Admin class
            Admin admin = new Admin(
                "admin@gmail.com",
                "admin123",
                "Manage Books"
            );

            // Display the admin's information
            Console.WriteLine("bring admin information");
            Console.WriteLine("\nAdmin:");
            Console.WriteLine($"admin email: {admin.Email}");
            Console.WriteLine($"admin password: {admin.Password} (the password is secret but displayed for testing purposes)");
            Console.WriteLine($"admin role: {admin.Role}");




            // ===== Test Order =====
            // Create an instance of the Order class
            Order order = new Order(customer1);


            Console.WriteLine("add order item");
            order.AddItem(book1, 2);// Add 2 copies of the book to the order


            // Display the order's information
            Console.WriteLine("bring order information");
            Console.WriteLine("\nOrder:");
            Console.WriteLine($"order ID: {order.Id}");

            Console.WriteLine(
                $"Customer: {order.Customer.Name}"
            );


            Console.WriteLine(
                $"Total: {order.CalculateTotal()}"
            );


            //=========WEEK2-DAY2============

            Console.WriteLine("\n\n================= WEEK2 - DAY2 =================");
            // Create an instance of the Book class
            Book book2 = new Book(
              "1984",
               30,
               15,
               author,
               translator
           );
            Book book3 = new Book(
               "animal farm",
               20,
               5,
               author,
               translator
           );
            //list of books
            List<Book> books = new List<Book>
            {
                book1,
                book2,
                book3,
                new Book(
                    "The Great Gatsby",
                    15,
                    8,
                    author,
                    translator
                ),
                new Book(
                    "To Kill a Mockingbird",
                    20,
                    12,
                    author,
                    translator
                ),
                new Book(
                    "Pride and Prejudice",
                    18,
                    6,
                    author,
                    translator
                ),
            };


            // Create a list of customers
            List<Customer> customers = new List<Customer>
            {
                new Customer("Leen", "leen@gmail.com", "12345", "customer"),
                new Customer("noor", "noor@gmail.com", "password123", "customer"),
                new Customer("rand", "rand@gmail.com", "password123", "customer"),
                new Customer("sara", "sara@gmail.com", "password123", "customer"),
                new Customer("firyal", "firyal@gmail.com", "password123", "customer"),
                new Customer("dana", "dana@gmail.com", "password123", "customer")
            };

            // Create a list of orders for each customer
            List<Order> orders = new List<Order>
            {
                new Order(customers[0]),
                new Order(customers[1]),
                new Order(customers[2]),
                new Order(customers[3]),
                new Order(customers[4]),
                new Order(customers[5]),
                new Order(customers[3]),
                new Order(customers[4]),
                new Order(customers[2]),
                new Order(customers[2]),
                new Order(customers[1]),
            };

            // Add items to each order
            orders[0].AddItem(book2, 2);
            orders[1].AddItem(book1, 4);
            orders[2].AddItem(book3, 1);
            orders[3].AddItem(book2, 3);
            orders[4].AddItem(book1, 6);
            orders[5].AddItem(book2, 2);
            orders[6].AddItem(book3, 1);
            orders[7].AddItem(book1, 2);
            orders[8].AddItem(book2, 3);
            orders[9].AddItem(book3, 5);
            orders[10].AddItem(book1, 2);


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