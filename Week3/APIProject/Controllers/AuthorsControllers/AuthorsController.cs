// This file contains the API Controller responsible for handling Author requests.
using APIProject.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIProject.Controllers.AuthorsControllers
{

    [ApiController]
    [Route("api/v1/authors")]
    public class AuthorsController : ControllerBase
    {


        private readonly IAuthorService _authorService;
        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET: api/v1/authors/{authorId}/books
        // This endpoint retrieves all books written by a specific author.
        [HttpGet("{authorId}/books")]
        public IActionResult GetAuthorBooks(int authorId)
        {
            var books = _authorService.GetBooksByAuthorId(authorId);

            if (!books.Any())
            {
                return NotFound($"No books found for author with id {authorId}");
            }

            return Ok(books);
        }

    }
}

