// this file is a controller for week 3 day 4 it contains a crud api for books
/*
 * For Week 4 day 3 :
 create book endpoint is protected by authorization
 update book endpoint is protected by policy
 delete book endpoint is protected by role
*/
using APIProject.Dto_s.BookDto_s.BookDto_sWeek3;
using APIProject.Interfaces.InterfacesWeek3;
using Microsoft.AspNetCore.Authorization;
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



        //  Get all books
        // GET: api/v1/day4/books
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {

            var books = await _bookService.GetAllBooksAsync();


            return Ok(books);

        }




        //  Get book by id 
        // GET: api/v1/day4/books/{id}
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




        //  Create book 
        // POST: api/v1/day4/books
        // This endpoint is protected by authorization, meaning that only authenticated users can access it.
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBook(CreateBookDto bookDto)
        {


            var createdBook = await _bookService.CreateBookAsync(bookDto);


            // Return a 201 Created response with the location of the newly created book
            return CreatedAtAction(
                nameof(GetBookById),
                new { id = createdBook.Id },
                createdBook
            );

        }





        //  Update book 
        // PUT: api/v1/day4/books/{id}
        /* 
          This endpoint is protected by a policy,
          meaning that only users who meet the requirements of the policy can access it.
        */
        [Authorize(Policy = "CanUpdateBook")]
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





        //  Delete book 
        // DELETE: api/v1/day4/books/{id}
        // This endpoint is protected by a admin role,
        [Authorize(Roles = "Admin")]
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
