using Microsoft.AspNetCore.Mvc;
using APIProject.Models;


namespace APIProject.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {

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

    }
}
