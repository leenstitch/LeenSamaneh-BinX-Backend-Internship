//this code defines a data transfer object (DTO) for updating a book.
namespace APIProject.Dto_s.BookDto_s
{
    public class UpdateBookDto
    {
        public string ? Title { get; set; }

        public string ? Description { get; set; }

        public decimal  ? Price { get; set; }

        public int ? AuthorId { get; set; }
    }
}
