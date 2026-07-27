using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Models
{
    // Author class inherits from Person
    public class Author : Person
    {
        public string AuthorBiography { get; private set; }


        public List<Book> Books { get; private set; } = new();


        // Constructor for Author class
        public Author(string name,string biography)
            : base(name)// Call the base class constructor to set the name
        {
            AuthorBiography = biography;
        }
    }
}
