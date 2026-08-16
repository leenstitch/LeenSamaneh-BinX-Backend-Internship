// a Translator model class to represent the Translator entity

namespace APIProject.Models
{
    public class Translator
    {
        public int Id { get; set; }

        public string ? Name { get; set; } 

        public string ? Language { get; set; } 

        public List<Book> Books { get; set; } = new();
    }
}
