// interface for implementing CRUD operations for Book entity in Week 3 - Day 4
using APIProject.Dto_s.BookDto_s.BookDto_sWeek3;

namespace APIProject.Interfaces.InterfacesWeek3
{
    public interface IBookServiceForDay4
    {
        // ======== Week3 - Day 4=========
        //========CRUD Operations by using Async ========
        Task<IEnumerable<BookResponseDto>> GetAllBooksAsync();

        Task<BookResponseDto?> GetBookByIdAsync(int id);

        Task<BookResponseDto> CreateBookAsync(CreateBookDto bookDto);

        Task<BookResponseDto?> UpdateBookAsync(int id, UpdateBookDto bookDto);

        Task<bool> DeleteBookAsync(int id);
    }
}
