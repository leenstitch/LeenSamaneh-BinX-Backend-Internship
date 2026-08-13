// this file is a service class that implements the IBookServiceForDay4 interface and provides methods for a crud operations .
using APIProject.Data;
using APIProject.Dto_s.BookDto_s.BookDto_sWeek3;
using APIProject.Interfaces.InterfacesWeek3;
using APIProject.Models;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Services1.ServicesForWeek3
{
    public class BookServiceForDay4 : IBookServiceForDay4
    {
        private readonly LibraryDbContext _context;


        public BookServiceForDay4(LibraryDbContext context)
        {
            _context = context;
        }

        // Get all books
        public async Task<IEnumerable<BookResponseDto>> GetAllBooksAsync()
        {
            return await _context.Books
                .Select(b => new BookResponseDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    Price = b.Price,
                    Quantity=b.Quantity,
                    AuthorId = b.AuthorId,
                    TranslatorId = b.TranslatorId,
                })
                .ToListAsync();
        }

        // Get a book by its ID

        public async Task<BookResponseDto?> GetBookByIdAsync(int id)
        {
            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id);


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
                AuthorId = book.AuthorId,
                TranslatorId = book.TranslatorId
            };
        }

        // create a new book

        public async Task<BookResponseDto> CreateBookAsync(CreateBookDto bookDto)
        {

            var book = new Book
            {
                Title = bookDto.Title,
                Description = bookDto.Description,
                Price = bookDto.Price,
                Quantity = bookDto.Quantity,
                AuthorId = bookDto.AuthorId,
                TranslatorId = bookDto.TranslatorId
            };


            _context.Books.Add(book);


            await _context.SaveChangesAsync();



            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                Quantity = book.Quantity,
                AuthorId = book.AuthorId,
                TranslatorId = book.TranslatorId
            };
        }

        // update an existing book

        public async Task<BookResponseDto?> UpdateBookAsync(int id, UpdateBookDto bookDto)
        {

            var existingBook = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id);


            
            if (existingBook == null)
            {
                return null;
            }

            // Update the properties of the existing book with the values from the DTO if they are not null

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



            if (bookDto.Quantity.HasValue)
            {
                existingBook.Quantity = bookDto.Quantity.Value;
            }



            if (bookDto.AuthorId.HasValue)
            {
                existingBook.AuthorId = bookDto.AuthorId.Value;
            }



            if (bookDto.TranslatorId.HasValue)
            {
                existingBook.TranslatorId = bookDto.TranslatorId.Value;
            }


            await _context.SaveChangesAsync();



            return new BookResponseDto
            {
                Id = existingBook.Id,
                Title = existingBook.Title,
                Description = existingBook.Description,
                Price = existingBook.Price,
                Quantity = existingBook.Quantity,
                AuthorId = existingBook.AuthorId,
                TranslatorId = existingBook.TranslatorId
            };
        }


        // Delete a book by its ID

        public async Task<bool> DeleteBookAsync(int id)
        {

            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id);



            if (book == null)
            {
                return false;
            }



            _context.Books.Remove(book);


            await _context.SaveChangesAsync();


            return true;
        }
    }
}
