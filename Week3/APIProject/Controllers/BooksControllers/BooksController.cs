// This file contains the API Controller responsible for handling book requests.

using APIProject.Data;
using APIProject.Dto_s.BookDto_s;
using APIProject.Interfaces;
using APIProject.Models;
using Microsoft.AspNetCore.Mvc;


namespace APIProject.Controllers
{

    [ApiController]
    //[Route("api/[controller]")] //for week 2

    [Route("api/v1/books")] //for week 3 with versioning
    public class BooksController : ControllerBase
    {

        private readonly IBookService _bookService;


        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        //======= for  week 3 - day 1  ========
        // GET: api/v1/books
        //Api to get all of the books
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            var books = _bookService.GetAllBooks();

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

            var book = _bookService.GetBookByItsId(id);

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
            var createdBook = _bookService.AddBook(bookDto);


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
                _bookService.UpdateBook(id, bookDto);


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

            var deleted = _bookService.DeleteBook(id);


            if (!deleted)
            {
                return NotFound();
            }


            return NoContent();

        }


        //======= for  week 2 - day 5  ========
        /*
        // GET: api/books
        //Api to get all of the books
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            var books = _bookService.GetBooks();

            return Ok(books);
        }



        // GET: api/books/1
        //Api to return the book with a specific id
        [HttpGet("{id}")]
        public ActionResult<Book> GetBookById(int id)
        {
            var book = _bookService.GetBookById(id);


            if (book == null)
            {
                return NotFound();
            }


            return Ok(book);
        }
        */






        /*   
        //======== for  week 2 - day 4  ========
        // Hardcoded data source
        private static List<Book> books = new()
        {
            new Book
            {
                Id = 1,
                Title = "Harry Potter",
                Description ="Amazing book",
                Price = 25
            },

            new Book
            {
                Id = 2,
                Title = "Clean Code",
               Description="Greate book ",
                Price = 40
            },

            new Book
            {
                Id = 3,
                Title = "The Hobbit",
                Description="Good book",
                Price = 30
            }
        };



       
        // ========= GET ALL BOOKS =========
       

        // GET: api/books
        //Api to get all of the books
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            return Ok(books);
        }




        //============= GET BOOK BY ID ============= 


        // GET: api/books/1
        //Api to return the book with a specific id
        [HttpGet("{id}")]
        public ActionResult<Book> GetBookById(int id)
        {
            //Searching for the book
            var book = books.FirstOrDefault(b => b.Id == id);


            if (book == null)
            {
                return NotFound();
            }


            return Ok(book);

        }
*/
    }
}
