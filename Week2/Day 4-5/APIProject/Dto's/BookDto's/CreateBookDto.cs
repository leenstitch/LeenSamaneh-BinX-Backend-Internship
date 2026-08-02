//this code defines a data transfer object (DTO) for creating a book.

using System.ComponentModel.DataAnnotations;

namespace APIProject.Dto_s.BookDto_s
{
    public class CreateBookDto
    {
        public string Title { get; set; }

        public string ?  Description { get; set; }

        [Range(0, 10000)]
        public decimal Price { get; set; }

        public int AuthorId { get; set; }
    }
}
