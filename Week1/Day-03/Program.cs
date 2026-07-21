using Library.Models;
using Library.Dtos;
using Library.Payments;

namespace Library
{
    public class Program
    {
        static void Main(string[] args)
        {
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
            Console.WriteLine($"Book title :  {book1.Title} - Book Author :  {book1.Author} - Book quantity : {book1.Quantity} - Book price :  { book1.Price} ");
            Console.WriteLine($"Book title :  {book2.Title} - Book Author :  {book2.Author} - Book quantity : {book2.Quantity} - Book price :  { book2.Price}");
           



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



            Console.WriteLine("\nFinished!");
        }
    }
}