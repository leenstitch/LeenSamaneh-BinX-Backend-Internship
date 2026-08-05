// a book model class to represent the Book entity
using System.ComponentModel.DataAnnotations;

namespace APIProject.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ? Description { get; set; }

        [Range(typeof(decimal), "0", "10000")]
        public decimal Price { get; set; } = 0;
        public int Quantity { get; set; } = 0;
        public int AuthorId { get; set; }
        public Author ? Author { get; set; }
        public int? TranslatorId { get; set; }

        public Translator? Translator { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}

