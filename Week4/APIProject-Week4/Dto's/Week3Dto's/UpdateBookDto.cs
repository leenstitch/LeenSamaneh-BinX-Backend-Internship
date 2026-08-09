// this file is used to defines a Data Transfer Object (DTO) for Updating a book entity.
//it was just for week 2
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
