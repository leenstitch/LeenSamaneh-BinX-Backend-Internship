/*
    WEEK2 - DAY3: Async/Await Deep Dive & Concurrency Basics

    Project Structure:
    
    - TestData:
        Contains LibrarySeedData which is responsible for creating and storing
        initial library data such as Authors, Translators, Books, Customers, and Orders.
        The data is created once and reused throughout the project.

    - Services:
        Contains LibraryDataServiceForWeek2Day3 which simulates asynchronous data sources.
        It provides async methods that return library data after a simulated delay
        using Task.Delay.

    - Week2/Day3:
        This file contains the practical implementation and testing of async concepts:
        
        1. Sequential execution:
           Calling async methods one by one using await and measuring execution time.

        2. Concurrent execution:
           Starting multiple independent tasks together and using Task.WhenAll
           to wait for all operations to complete.

        3. Cancellation Token:
           Passing a CancellationToken to an async operation and cancelling
           the operation before it finishes.

    The goal of this day is to understand how asynchronous programming works,
    how multiple independent operations can run concurrently, and how long-running
    operations can be cancelled when needed.
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Models;
using LibrarySystem.Services;
using LibrarySystem.TestData;
using System.Diagnostics;
using System.ComponentModel;
namespace LibrarySystem.Week2
{
    internal class Day3
    {
        public static async Task Week2Day3()
        {
            Console.WriteLine("\n\n================= WEEK2 - DAY3 =================");

            // ==================================================
            // Part 1:
            // Sequential execution
            // Each task waits for the previous one to finish
            // ==================================================


            LibraryDataServiceForWeek2Day3 service = new LibraryDataServiceForWeek2Day3();


            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            Console.WriteLine("\nSequential executionl");
            // Sequential execution
            // First task starts and waits until books are loaded
            var books = await service.GetBooksAsync();
            Console.WriteLine($"Books count: {books.Count}");

            // Second task starts after books task finishes
            var customers = await service.GetCustomersAsync();
            Console.WriteLine($"Customers count: {customers.Count}");

            // Third task starts after customers task finishes
            var orders = await service.GetOrdersAsync();
            Console.WriteLine($"Orders count: {orders.Count}");

            stopwatch.Stop();

            Console.WriteLine(
                $"Sequential Time: {stopwatch.ElapsedMilliseconds} ms"
            );





            // ==================================================
            // Part 2:
            // Concurrent execution using Task.WhenAll
            // All tasks start at the same time
            // ==================================================

            Console.WriteLine("\nBy using Task.WhenAll");
            LibraryDataServiceForWeek2Day3 service1 = new LibraryDataServiceForWeek2Day3();

            Stopwatch stopwatch1 = new Stopwatch();


            stopwatch1.Start();
            // Start all asynchronous operations immediately
            // They run independently without waiting for each other
            Task<List<Book>> booksTask = service1.GetBooksAsync();

            Task<List<Customer>> customersTask = service1.GetCustomersAsync();

            Task<List<Order>> ordersTask = service1.GetOrdersAsync();



            // Wait until all started tasks are completed
            // Task.WhenAll combines multiple tasks into one task

            await Task.WhenAll(
                booksTask,
                customersTask,
                ordersTask
            );


            // Retrieve the results after all tasks are completed
            var books1 = await booksTask;
            Console.WriteLine($"Books count: {books1.Count}");

            var customers1 = await customersTask;
            Console.WriteLine($"Customers count: {customers1.Count}");

            var orders1 = await ordersTask;
            Console.WriteLine($"Orders count: {orders1.Count}");


            stopwatch1.Stop();

            Console.WriteLine(
                $"Concurrent Time: {stopwatch1.ElapsedMilliseconds} ms"
            );




            // ==================================================
            // Part 3:
            // CancellationToken example
            // ==================================================

            Console.WriteLine("\nCancellation Token");

            LibraryDataServiceForWeek2Day3 service2 = new LibraryDataServiceForWeek2Day3();


            // CancellationTokenSource creates a token
            // that can send a cancellation request
            CancellationTokenSource cts = new CancellationTokenSource();

            // Start loading books and pass the cancellation token
            var booksTask2 = service2.GetBooksWithCancellationAsync(cts.Token);


            // Wait for 1 second before cancelling
            // This simulates a user cancelling a request
            await Task.Delay(1000);


            // Request cancellation
            cts.Cancel();



            // If cancellation happens,
            // this await will throw OperationCanceledException
            try
            {

                var books2 = await booksTask2;

                Console.WriteLine($"Books count: {books2.Count}");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Books loading was cancelled");
            }
            


        }
    
}
}