using System.ComponentModel.DataAnnotations;

namespace APIProject.Dto_s.BookDto_s.BookDto_sWeek3
{
    public class UpdateBookDto
    {
       
        public string ? Title { get; set; } 
        public string? Description { get; set; }

        [Range(typeof(decimal), "0", "10000")]
        public decimal ? Price { get; set; } 
        public int ? Quantity { get; set; }
        public int ? AuthorId { get; set; }
        public int ? TranslatorId { get; set; }

    }
}
