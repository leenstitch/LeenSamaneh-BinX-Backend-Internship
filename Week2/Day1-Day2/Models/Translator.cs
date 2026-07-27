using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Models
{
    // Translator class inherits from Person
    public class Translator : Person
    {
        public string Language { get; private set; }


        public List<Book> Books { get; private set; } = new();

        // Constructor for Translator class
        public Translator(string name,string language)
            : base(name)
        {
            Language = language;
        }
    }
}
