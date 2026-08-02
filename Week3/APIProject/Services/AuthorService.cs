// This file contains the implementation of IAuthorService.
// It contains the actual logic for retrieving authors.
using APIProject.Data;
using APIProject.Interfaces;
using APIProject.Models;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Services
{
    public class AuthorService : IAuthorService
    {
        // AuthorService implements the IAuthorService contract.
        private readonly LibraryDbContext _context;

        // authorService constructor that takes a LibraryDbContext as a parameter and assigns it to the _context field.
        public AuthorService(LibraryDbContext context)
        {
            _context = context;
        }


        // This method retrieves all books written by a specific author based on the provided authorId.
        public IEnumerable<Book> GetBooksByAuthorId(int authorId)
        {

            // The method uses Entity Framework Core to query the database for books that have a matching AuthorId.
            // It includes the Author navigation property to load related author data along with the books.
            // Finally, it returns the list of books as an IEnumerable<Book>.
            return _context.Books
               .Where(b => b.AuthorId == authorId)
               .Include(b => b.Author)
               .ToList();

        }

    }
}