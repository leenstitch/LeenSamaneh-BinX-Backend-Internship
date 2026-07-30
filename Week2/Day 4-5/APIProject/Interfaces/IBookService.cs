// This file defines the contract for book-related operations.
// Any class implementing this interface must provide these methods.
using APIProject.Models;

namespace APIProject.Interfaces
{
    // Interface defines what the service should do,
    // without defining how it does it.
    public interface IBookService
    {

        // Method that returns all books.
        IEnumerable<Book> GetBooks();

        // Method that returns one book by ID.
        Book? GetBookById(int id);
    }
}