using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Models;

namespace LibrarySystem.Repositories
{
    // BookRepository class inherits from Repository<Book> and provides additional methods for querying books
    public class BookRepository
     : Repository<Book>
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
