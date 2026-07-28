/*
    File: BookRepository.cs

    Purpose:
    This file provides book-specific repository operations.

    Responsibility:
    - Inherits the common functionality from Repository<Book>.
    - Adds extra queries related only to books.

    Used Files:
    - Repository<Book> as the base repository.
    - Book model as the managed entity.

    Day 1 Concepts Applied:
    - Reusing generic repository functionality.
    - Applying generics with a specific model type.
    - Extending existing generic classes.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Models;

namespace LibrarySystem.Repositories
{
    // BookRepository class inherits from Repository<Book> and provides additional methods for querying books
    public class BookRepository : Repository<Book>
    {

        // Method to get books with a price above a specified value
        public IEnumerable<Book> GetBooksAbovePrice(decimal price)
        {
            return GetAll()
                .Where(x => x.Price > price);
        }

        // Method to get books by a specific author
        public IEnumerable<Book> GetBooksByTitle(string title)
        {
            return GetAll()
                .Where(x => x.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

    }
}
