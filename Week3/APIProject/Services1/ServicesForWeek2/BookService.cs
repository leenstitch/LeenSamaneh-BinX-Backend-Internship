// This file contains the implementation of IBookService.
// It contains the actual logic for retrieving books.
using APIProject.Data;
using APIProject.Dto_s.BookDto_s;
using APIProject.Interfaces.InterfacesForWeek2;
using APIProject.Models;
using Microsoft.EntityFrameworkCore;
namespace APIProject.Services1.Services
{
    // BookService implements the IBookService contract.
    public class BookService : IBookService
    {

        // The BookService class is responsible for handling book-related operations.
        private readonly LibraryDbContext _context;

        // BookService constructor that takes a LibraryDbContext as a parameter and assigns it to the _context field.
        public BookService(LibraryDbContext context)
        {
            _context = context;
        }


        //========= week 2  =========
        /*
        // Returns all books.
        public IEnumerable<Book> GetBooks()
        {
            return books;
        }


        // Searches for a book using its ID.
        public Book? GetBookById(int id)
        {
             var book = _context.Books
                .FirstOrDefault(b => b.Id == id);
            // Find the first book that matches the provided ID.
            // If no book exists, FirstOrDefault returns null.
            return books.FirstOrDefault(b => b.Id == id);
        }
*/
   
    }
}