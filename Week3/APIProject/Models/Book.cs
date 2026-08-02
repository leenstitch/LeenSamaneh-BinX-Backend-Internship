// a author model class to represent the Book entity
using System.ComponentModel.DataAnnotations;

namespace APIProject.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ? Description { get; set; }

        [Range(0, 10000)]
        public decimal Price { get; set; }
        public int AuthorId { get; set; }
        public Author ? Author { get; set; }
    }
}
