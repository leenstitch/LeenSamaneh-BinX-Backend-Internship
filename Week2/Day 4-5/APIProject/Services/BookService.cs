// This file contains the implementation of IBookService.
// It contains the actual logic for retrieving books.
using APIProject.Interfaces;
using APIProject.Models;

namespace APIProject.Services
{
    // BookService implements the IBookService contract.
    public class BookService : IBookService
    {
        // Private list that stores book data.
        private readonly List<Book> books = new()
        {
            new Book
            {
                Id = 1,
                Title = "Harry Potter",
                Description = "Amazing book",
                Price = 25
            },

            new Book
            {
                Id = 2,
                Title = "Clean Code",
                Description = "Great book",
                Price = 40
            },

            new Book
            {
                Id = 3,
                Title = "The Hobbit",
                Description = "Good book",
                Price = 30
            }
        };

        // Returns all books.
        public IEnumerable<Book> GetBooks()
        {
            return books;
        }


        // Searches for a book using its ID.
        public Book? GetBookById(int id)
        {

            // Find the first book that matches the provided ID.
            // If no book exists, FirstOrDefault returns null.
            return books.FirstOrDefault(b => b.Id == id);
        }

    }
}