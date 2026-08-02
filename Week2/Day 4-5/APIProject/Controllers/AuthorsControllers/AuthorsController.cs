// This file contains the API Controller responsible for handling Author requests.
using APIProject.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIProject.Controllers.AuthorsControllers
{
    [Route("api/v1/authors")]
    public class AuthorsController : ControllerBase
    {


        private readonly IAuthorService _authorService;
        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        //======= for  week 3 - day 1  ========


        // GET: api/v1/authors/{authorId}/books
        // This endpoint retrieves all books written by a specific author.
        [HttpGet("{authorId}/books")]
        public IActionResult GetAuthorBooks(int authorId)
        {
            var books = _authorService.GetBooksByAuthorId(authorId);

            return Ok(books);
        }

    }
}

