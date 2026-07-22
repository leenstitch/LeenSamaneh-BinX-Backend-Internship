using Library.Models;
using Library.Dtos;
using Library.Payments;

namespace Library
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            //====== Day 3 - Library System ======
            Console.WriteLine("\nDay 3 - Library System");
            //  Create Books
            Book book1 = new Book(
                "Harry Potter",
                "J.K. Rowling",
                10,
                25m
            );

            Book book2 = new Book(
                "Death on the Nile",
                "Agatha Christie",
                5,
                30m
            );


            Console.WriteLine(" The Books That have been created:");
            Console.WriteLine($"Book title :  {book1.Title} - Book Author :  {book1.Author} - Book quantity : {book1.Quantity} - Book price :  {book1.Price} ");
            Console.WriteLine($"Book title :  {book2.Title} - Book Author :  {book2.Author} - Book quantity : {book2.Quantity} - Book price :  {book2.Price}");




            //  Create Customer
            Customer customer = new Customer("Leen");

            Console.WriteLine("\nNew Customer created:");
            Console.WriteLine($"Customer name : {customer.Name}");



            //  Create Order
            Order order = new Order(customer);

            Console.WriteLine("\nNew Order has been created:");
            Console.WriteLine($"Order Id : {order.Id}");

            Console.WriteLine($"Customer Orders Count : {customer.Orders.Count}");



            //  Add Books to Order
            order.AddBook(book1, 2);
            order.AddBook(book2, 1);


            Console.WriteLine("\nBooks in Order:");

            foreach (var item in order.BookOrders)
            {
                Console.WriteLine(
                    $"Book Title : {item.Book.Title} - Quantity: {item.Quantity}"
                );
            }



            //  Calculate Total Price
            decimal total = order.CalculateTotal();

            Console.WriteLine("\nOrder Total price:");
            Console.WriteLine(total);



            //  Create OrderDto (Record)
            OrderDto orderDto = new OrderDto(
                order.Id,
                customer.Name,
                total
            );


            Console.WriteLine("\nOrder DTO:");
            Console.WriteLine($"Order Id: {orderDto.Id}");
            Console.WriteLine($"Customer name: {orderDto.CustomerName}");
            Console.WriteLine($"Order Total price: {orderDto.TotalPrice}");



            //  Polymorphism - Payment Interface

            Console.WriteLine("\nCash Payment:");

            CashPayment cashPayment = new CashPayment();

            PaymentResult cashResult = order.ProcessPayment(cashPayment);

            Console.WriteLine(cashResult.Message);
            Console.WriteLine($"Payment Method: {cashResult.PaymentMethod}");
            Console.WriteLine($"Amount: {cashResult.Amount}");



            Console.WriteLine("\nCard Payment:");

            CardPayment cardPayment = new CardPayment();

            PaymentResult cardResult = order.ProcessPayment(cardPayment);

            Console.WriteLine(cardResult.Message);
            Console.WriteLine($"Payment Method: {cardResult.PaymentMethod}");
            Console.WriteLine($"Amount: {cardResult.Amount}");


            Console.WriteLine("\nDay 3 Finished!");




            //======= Day 4 - Library System ======
            Console.WriteLine("\nDay 4 - Library System");
            // Create a list of  8 books
            List<Book> books = new List<Book> { book1, book2 };
            books.Add(new Book("The Great Gatsby", "F. Scott Fitzgerald", 7, 20m));
            books.Add(new Book("To Kill a Mockingbird", "Harper Lee", 12, 18m));
            books.Add(new Book("1984", "George Orwell", 8, 22m));
            books.Add(new Book("Pride and Prejudice", "Jane Austen", 15, 19m));
            books.Add(new Book("The Catcher in the Rye", "J.D. Salinger", 6, 21m));
            books.Add(new Book("The Hobbit", "J.R.R. Tolkien", 9, 24m));
            books.Add(new Book("The Lord of the Rings", "J.R.R. Tolkien", 4, 35m));
            books.Add(new Book("The Chronicles of Narnia", "C.S. Lewis", 11, 28m));


            //=========== LINQ ===========
            // Use LINQ to filter books with price above 20
           
            var BooksWithPriceAbove20 = books.Where(b => b.Price > 20)
                                        .OrderBy(b => b.Price)
                                        .ToList();

            // Display the filtered books
            Console.WriteLine("\nBooks with price above 20:");
            foreach (var book in BooksWithPriceAbove20)
            {
                Console.WriteLine($"Book Title: {book.Title}, Price: {book.Price}");
            }

            // Use LINQ to select only the book titles
            var BooksName = books.Select(b => b.Title)
                                 .OrderBy(b => b)
                                 .ToList();

            // Display the book titles
            Console.WriteLine("\nBook Titles:");
            foreach (var bookName in BooksName)
            {
                Console.WriteLine($"Book Title: {bookName}");
            }

            
            // Use LINQ to calculate the average price of books
            var BooksPriceAverage = books.Average(b => b.Price);
            // Display the average price of books
            Console.WriteLine($"\nAverage Price of Books: {BooksPriceAverage}");



            // use async method to simulate loading books with a delay of 3 seconds
            Console.WriteLine("\nLoading books by using async method...");
            string result = await LoadBooksAsync();
             Console.WriteLine(result);



            // Add a new book to the list and handle exceptions for invalid input by using try-catch block
            Console.WriteLine("\nAdd a new book to the list:");
            Console.Write("Enter book title: ");
            string title = Console.ReadLine();

            Console.Write("Enter author: ");
            string author = Console.ReadLine();

            Console.Write("Enter quantity: ");
            int quantity = int.Parse(Console.ReadLine());

            // Use try-catch block to handle exceptions for invalid  price input
            try
            {
                Console.Write("Enter price: ");
                string priceInput = Console.ReadLine();
                decimal price = decimal.Parse(priceInput);

                Book book = new Book(title, author, quantity, price);

                books.Add(book);

                Console.WriteLine("Book added successfully");
            }
            catch (FormatException)
            {
                Console.WriteLine("Quantity and price must be valid numbers.");
            }

        }

            // This method simulates loading books asynchronously, with a delay of 3 seconds, and returns a success message.
            private static async Task<string> LoadBooksAsync()
        {
            await Task.Delay(3000);

            return "Books loaded successfully";
        }

        
        }
    }
