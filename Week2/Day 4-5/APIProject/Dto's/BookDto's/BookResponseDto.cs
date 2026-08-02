// this code is a C# class definition for a Data Transfer Object (DTO) named `BookResponseDto`.
// it used to return a book's details in a response

namespace APIProject.Dto_s.BookDto_s
{
    public class BookResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ? Description { get; set; }

        public decimal Price { get; set; }

        public int AuthorId { get; set; }
    }
}
