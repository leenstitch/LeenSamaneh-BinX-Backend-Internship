// This file contains the API Controller responsible for handling book requests.

using APIProject.Data;
using APIProject.Dto_s.BookDto_s.BookDto_sWeek3;

using APIProject.Interfaces.InterfacesWeek3;

using APIProject.Models;
using Microsoft.AspNetCore.Mvc;


namespace APIProject.Controllers
{

    [ApiController]


    [Route("api/v1/day1/books")] //for week 3 with versioning
    public class BooksController1 : ControllerBase
    {

        private readonly IBookServiceForDay1 _bookService1;


        public BooksController1(IBookServiceForDay1 bookService)
        {
            _bookService1 = bookService;
        }

        //======= for  week 3 - day 1  ========
        // GET: api/v1/books
        //Api to get all of the books
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            var books = _bookService1.GetAllBooks();

            if (books == null || !books.Any())
            {
                return NotFound("No books found.");
            }


            return Ok(books);
        }

        // GET: api/v1/books/1
        //Api to return the book with a specific id
        [HttpGet("{id}")]
        public ActionResult<Book> GetBookByItsId(int id)
        {

            var book = _bookService1.GetBookByItsId(id);

            if (book == null)
            {
                return NotFound("Book not found");
            }


            return Ok(book);
        }


        //post: api/v1/books
        //Api to add a new book
        [HttpPost]
        public ActionResult<Book> AddBook(CreateBookDto bookDto)
        {
            var createdBook = _bookService1.AddBook(bookDto);


            return CreatedAtAction(
                nameof(GetBookByItsId),
                new { id = createdBook.Id },
                createdBook
            );
        }

        //put: api/v1/books/1
        //Api to update an existing book
        [HttpPut("{id}")]
        public IActionResult UpdateBook(
      int id,
      UpdateBookDto bookDto)
        {

            var updatedBook =
                _bookService1.UpdateBook(id, bookDto);


            if (updatedBook == null)
            {
                return NotFound(
                    $"Book with id {id} was not found"
                );
            }


            return Ok(updatedBook);
        }

        //delete: api/v1/books/1
        //Api to delete a book by id
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {

            var deleted = _bookService1.DeleteBook(id);


            if (!deleted)
            {
                return NotFound();
            }


            return NoContent();

        }


        





    }
}
