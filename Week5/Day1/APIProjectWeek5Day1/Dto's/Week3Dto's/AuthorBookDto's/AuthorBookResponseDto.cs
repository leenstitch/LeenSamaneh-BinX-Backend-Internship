// this dto is used as a response model for author and book entity.

namespace APIProject.Dto_s.Week3Dto_s.AuthorBookDto_s
{
    public class AuthorBookResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } 

        public decimal Price { get; set; }
        public string ? Description { get; set; }
        public decimal Quantity { get; set; }
}
}
