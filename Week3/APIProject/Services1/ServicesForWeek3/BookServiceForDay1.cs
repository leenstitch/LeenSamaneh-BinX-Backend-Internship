// this file is a service class that implements the IBookServiceForDay1 interface and provides methods for a crud operations .
using APIProject.Data;
using APIProject.Dto_s.BookDto_s.BookDto_sWeek3;
using APIProject.Interfaces.InterfacesWeek3;

using APIProject.Models;

namespace APIProject.Services1.ServicesForWeek3
{
    public class BookServiceForDay1 : IBookServiceForDay1
    {

        // The BookService class is responsible for handling book-related operations.
        private readonly LibraryDbContext _context;

        // BookService constructor that takes a LibraryDbContext as a parameter and assigns it to the _context field.
        public BookServiceForDay1(LibraryDbContext context)
        {
            _context = context;
        }

        //========= week 3 day 1 =========


        // Returns all books from the database, including their associated authors.
        public IEnumerable<BookResponseDto> GetAllBooks()
        {

            return _context.Books
                .Select(b => new BookResponseDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    Price = b.Price,
                    Quantity= b.Quantity,
                    AuthorId = b.AuthorId
                })
                .ToList();
        }



        // Searches for a book using its ID and returns it, including its associated author.
        public BookResponseDto? GetBookByItsId(int id)
        {
            var book = _context.Books
                .FirstOrDefault(b => b.Id == id);


            if (book == null)
            {
                return null;
            }


            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                Quantity = book.Quantity,
                AuthorId = book.AuthorId
            };
        }


        // Adds a new book to the database and saves the changes.
        public BookResponseDto AddBook(CreateBookDto bookDto)
        {
            var book = new Book
            {
                Title = bookDto.Title,
                Description = bookDto.Description,
                Price = bookDto.Price,
                Quantity = bookDto.Quantity,
                AuthorId = bookDto.AuthorId
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                Quantity = book.Quantity,
                AuthorId = book.AuthorId
            };
        }


        // Updates an existing book in the database. If the book does not exist, it returns null.
        public BookResponseDto? UpdateBook(int id, UpdateBookDto bookDto)
        {
            var existingBook = _context.Books
                .FirstOrDefault(b => b.Id == id);


            if (existingBook == null)
            {
                return null;
            }


            if (bookDto.Title != null)
            {
                existingBook.Title = bookDto.Title;
            }


            if (bookDto.Description != null)
            {
                existingBook.Description = bookDto.Description;
            }


            if (bookDto.Price.HasValue)
            {
                existingBook.Price = bookDto.Price.Value;
            }
            if(bookDto.Quantity.HasValue)
            {
                existingBook.Quantity = bookDto.Quantity.Value;
            }

            if (bookDto.AuthorId.HasValue)
            {
                existingBook.AuthorId = bookDto.AuthorId.Value;
            }


            _context.SaveChanges();


            return new BookResponseDto
            {
                Id = existingBook.Id,
                Title = existingBook.Title,
                Description = existingBook.Description,
                Price = existingBook.Price,
                Quantity = existingBook.Quantity,
                AuthorId = existingBook.AuthorId
            };
        }



        // Deletes a book from the database using its ID. Returns true if the deletion was successful, or false if the book was not found.
        public bool DeleteBook(int id)
        {
            var book =
                _context.Books.FirstOrDefault(b => b.Id == id);


            if (book == null)
            {
                return false;
            }


            _context.Books.Remove(book);

            _context.SaveChanges();


            return true;
        }

    }
}