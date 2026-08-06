// interface for implementing CRUD operations for Book entity in Week 3 - Day 1
using APIProject.Dto_s.BookDto_s.BookDto_sWeek3;

namespace APIProject.Interfaces.InterfacesWeek3
{
    public interface IBookServiceForDay1
    {
        // ============== week 3 ==============

        //========= Week 3 - Day 1 ========
        //========CRUD Operations========
        IEnumerable<BookResponseDto> GetAllBooks();
        BookResponseDto? GetBookByItsId(int id);
        BookResponseDto AddBook(CreateBookDto bookDto);
        BookResponseDto? UpdateBook(int id, UpdateBookDto bookDto);
        bool DeleteBook(int id);


        
    }
}
