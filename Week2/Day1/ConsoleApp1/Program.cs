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
            Console.WriteLine($"author Biography: { author.AuthorBiography}");



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
            Book book = new Book(
                "Harry Potter",
                25,
                10,
                author,
                translator
            );

            // Display the book's information
            Console.WriteLine("bring book information");
            Console.WriteLine("\nBook:");
            Console.WriteLine($"book title: {book.Title}");
            Console.WriteLine($"book price: {book.Price}");
            Console.WriteLine($"book quantity: {book.Quantity}");

            Console.WriteLine(
                $"Author: {book.Author.Name}"
            );

            Console.WriteLine(
                $"Translator: {book.Translator.Name}"
            );



            // ===== Test Customer =====
            Console.WriteLine("\nTesting Customer class(add customer)");
            // Create an instance of the Customer class
            Customer customer = new Customer(
                "Leen",
                "leen@gmail.com",
                "12345",
                "customer"
            );
            // Display the customer's information
            Console.WriteLine("bring customer information");
            Console.WriteLine("\nCustomer:");
            Console.WriteLine($"customer name: {customer.Name}");
            Console.WriteLine($"customer email: {customer.Email}");
            Console.WriteLine($"customer password: {customer.Password} (the password is secret but displayed for testing purposes)");
            Console.WriteLine($"customer role: {customer.Role}");



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
            Order order = new Order(customer);

            
            Console.WriteLine("add order item");
            order.AddItem(book, 2);// Add 2 copies of the book to the order


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


            Console.ReadLine();
        }
    }
}

