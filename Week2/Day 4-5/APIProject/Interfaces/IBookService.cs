using APIProject.Models;

namespace APIProject.Interfaces
{
    public interface IBookService
    {
        IEnumerable<Book> GetBooks();

        Book? GetBookById(int id);
    }
}