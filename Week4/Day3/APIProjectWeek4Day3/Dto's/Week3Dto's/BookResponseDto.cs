//its just for week 2
// its used as a response model for the book entity.
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
