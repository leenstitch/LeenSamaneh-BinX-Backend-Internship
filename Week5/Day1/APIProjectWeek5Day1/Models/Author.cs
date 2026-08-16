// a author model class to represent the author entity .
using System.Text.Json.Serialization;

namespace APIProject.Models
{
    public class Author
    {


        public int  Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ? Bio { get; set; }
        public string ? Nationality { get; set; }
        [JsonIgnore]
        public List<Book> Books { get; set; } = new();
    }
}
