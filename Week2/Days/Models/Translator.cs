/*
    File: Translator.cs

    Purpose:
    This file represents translators in the library system.

    Responsibility:
    - Stores translator information.
    - Maintains books translated by the translator.

    Used Files:
    - Person:
      Provides common person information.
    - Book:
      Represents translated books.

    Concepts Applied:
    - Inheritance
    - Object relationships
*/
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
