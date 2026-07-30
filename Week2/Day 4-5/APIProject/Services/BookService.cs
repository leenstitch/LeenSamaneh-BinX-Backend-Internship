using APIProject.Interfaces;
using APIProject.Models;

namespace APIProject.Services
{
    public class BookService : IBookService
    {

        private readonly List<Book> books = new()
        {
            new Book
            {
                Id = 1,
                Title = "Harry Potter",
                Description = "Amazing book",
                Price = 25
            },

            new Book
            {
                Id = 2,
                Title = "Clean Code",
                Description = "Great book",
                Price = 40
            },

            new Book
            {
                Id = 3,
                Title = "The Hobbit",
                Description = "Good book",
                Price = 30
            }
        };


        public IEnumerable<Book> GetBooks()
        {
            return books;
        }



        public Book? GetBookById(int id)
        {
            return books.FirstOrDefault(b => b.Id == id);
        }

    }
}