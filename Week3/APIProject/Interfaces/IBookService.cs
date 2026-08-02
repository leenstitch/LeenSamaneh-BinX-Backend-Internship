// This file defines the contract for book-related operations.
// Any class implementing this interface must provide these methods.
using APIProject.Dto_s.BookDto_s;
using APIProject.Models;

namespace APIProject.Interfaces
{
    // Interface defines what the service should do,
    // without defining how it does it.
    public interface IBookService
    {
        /*
        // ======== week 2 ========
        
        // Method that returns all books.
        IEnumerable<Book> GetBooks();

        // Method that returns one book by ID.
        Book? GetBookById(int id);
        */


        // ======== week 3 ========
        //========CRUD Operations========
        IEnumerable<BookResponseDto> GetAllBooks();
        BookResponseDto AddBook(CreateBookDto bookDto);
        BookResponseDto ? UpdateBook(int id, UpdateBookDto bookDto);
        bool DeleteBook(int id);
        BookResponseDto? GetBookByItsId(int id);


    }
}