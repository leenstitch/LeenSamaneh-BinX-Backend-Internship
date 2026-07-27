using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Models
{
    // Person class represents a person with an Id and Name that can be inherited by other classes like Author and Translater.
    public abstract class Person
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        // Constructor for Person class
        protected Person(string name)
        {
            Id = Guid.NewGuid();

            Name = name ??
            throw new ArgumentNullException(nameof(name)+" cannot be null.");
        }
    }
}
