// This file contains the API Controller responsible for handling Author requests.
using APIProject.Interfaces.InterfacesWeek3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIProject.Controllers.Day3Controllers.AuthorsControllers
{

    [ApiController]
    [Route("api/v1/day1/authors")]
    public class AuthorsControllerDay1 : ControllerBase
    {


        private readonly IAuthorService _authorService;
        public AuthorsControllerDay1(IAuthorService authorService)
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

