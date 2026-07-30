// This file contains the API Controller responsible for handling book requests.

using Microsoft.AspNetCore.Mvc;
using APIProject.Models;
using APIProject.Interfaces;


namespace APIProject.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {

        // Private field that stores the injected service.
        private readonly IBookService _bookService;


        // Constructor Injection
        // ASP.NET Core automatically provides an IBookService object here.
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;// Store the received service inside the private field.
        }



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




        /*   
        //======== for day 4 ========
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
