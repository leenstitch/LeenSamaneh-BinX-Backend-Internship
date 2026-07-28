/*
    File: IBookService.cs

    Purpose:
    This file defines the contract for book service operations.

    Responsibility:
    - Defines the operations that a book service should provide.
    - Separates business logic from data access.

    Used Files:
    - BookService implements this interface.
    - Book model is used as the main entity.

    Concepts Applied:
    - Interface abstraction.
    - Separation of responsibilities.
    - Service Layer Pattern.

    Relation to Day 1:
    - Uses the repository structure created using generics.
    - Provides a higher-level layer above Repository<Book>.
*/

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
