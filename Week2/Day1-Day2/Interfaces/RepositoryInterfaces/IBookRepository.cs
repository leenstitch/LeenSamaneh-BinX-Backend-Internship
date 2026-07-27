using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Models;

namespace LibrarySystem.Interfaces.RepositoryInterfaces
{
    // This interface defines the contract for a book service that provides methods to add and retrieve books.
    public interface IBookService
    {

        void AddBook(Book book);// Method to add a book to the service.


        IReadOnlyList<Book> GetBooks();// Method to retrieve a read-only list of all books in the service.

    }
}
