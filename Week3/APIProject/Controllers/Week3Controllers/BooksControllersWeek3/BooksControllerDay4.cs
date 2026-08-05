using APIProject.Dto_s.BookDto_s.BookDto_sWeek3;
using APIProject.Interfaces.InterfacesWeek3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIProject.Controllers.BooksControllers.BooksControllersWeek3
{
    [Route("api/v1/day4/books")]
    [ApiController]
    public class BooksController4 : ControllerBase
    {
        private readonly IBookServiceForDay4 _bookService;


        public BooksController4(IBookServiceForDay4 bookService)
        {
            _bookService = bookService;
        }



        // ================= GET ALL =================

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {

            var books = await _bookService.GetAllBooksAsync();


            return Ok(books);

        }




        // ================= GET BY ID =================

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {

            var book = await _bookService.GetBookByIdAsync(id);



            if (book == null)
            {
                return NotFound(new
                {
                    message = "Book not found"
                });
            }



            return Ok(book);
        }




        // ================= CREATE =================

        [HttpPost]
        public async Task<IActionResult> CreateBook(CreateBookDto bookDto)
        {


            var createdBook = await _bookService.CreateBookAsync(bookDto);



            return CreatedAtAction(
                nameof(GetBookById),
                new { id = createdBook.Id },
                createdBook
            );

        }





        // ================= UPDATE =================

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(
            int id,
            UpdateBookDto bookDto)
        {


            var updatedBook = await _bookService
                .UpdateBookAsync(id, bookDto);



            if (updatedBook == null)
            {
                return NotFound(new
                {
                    message = "Book not found"
                });
            }



            return Ok(updatedBook);

        }





        // ================= DELETE =================

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {


            var deleted = await _bookService
                .DeleteBookAsync(id);



            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Book not found"
                });
            }



            return NoContent();

        }
    }
}
