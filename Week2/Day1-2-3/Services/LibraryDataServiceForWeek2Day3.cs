/*
    File: LibraryDataServiceForWeek2Day3.cs

    Purpose:
    This file simulates asynchronous data loading operations.

    Responsibility:
    - Provides async methods for loading Books, Customers, and Orders.
    - Simulates external data sources using Task.Delay.
    - Demonstrates cancellation using CancellationToken.

    Used Files:
    - LibrarySeedData:
      Provides the sample library data.
    - Models:
      Defines returned entities.

    Concepts Applied:
    - async/await
    - Task
    - Task.Delay
    - Task.WhenAll
    - CancellationToken
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Models;
using LibrarySystem.TestData;

namespace LibrarySystem.Services
{
    // Service class responsible for simulating data loading operations
    // It demonstrates asynchronous programming using Task and async/await
    public class LibraryDataServiceForWeek2Day3
    {
        //Async method that simulates loading books from a data source
        public async Task<List<Book>> GetBooksAsync()
        {
            Console.WriteLine("Getting books...");

            // Simulate database/API delay
            await Task.Delay(3000);

            Console.WriteLine("Books loaded");

            // Return books data from seed data
            return LibrarySeedData.Books;
        }


        // Async method that simulates loading customers from a data source
        public async Task<List<Customer>> GetCustomersAsync()
        {
            Console.WriteLine("Getting customers...");

            // Simulate database/API delay
            await Task.Delay(2000);

            Console.WriteLine("Customers loaded");

            // Return customers data from seed data
            return LibrarySeedData.Customers;
        }


        // Async method that simulates loading orders from a data source
        public async Task<List<Order>> GetOrdersAsync()
        {
            Console.WriteLine("Getting orders...");

            // Simulate database/API delay
            await Task.Delay(4000);

            Console.WriteLine("Orders loaded");

            // Return Order data from seed data
            return LibrarySeedData.Orders;
        }


        // Async method that supports cancellation
        // The operation can be stopped before completion using CancellationToken
        public async Task<List<Book>> GetBooksWithCancellationAsync(
            CancellationToken cancellationToken)
        {
            Console.WriteLine("Getting books with cancellation...");

            // Task.Delay accepts CancellationToken
            // If cancellation is requested, it throws OperationCanceledException
            await Task.Delay(
                3000,
                cancellationToken
            );

            Console.WriteLine("Books loaded");

            return LibrarySeedData.GetBooks();
        }
    }
}
