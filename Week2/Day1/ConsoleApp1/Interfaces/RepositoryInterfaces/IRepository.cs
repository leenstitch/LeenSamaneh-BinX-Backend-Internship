using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Interfaces.RepositoryInterfaces
{
    // This interface represents a generic repository for managing entities of type T.
    public interface IRepository<T>
     where T : class // Constraint that specifies T must be a reference type (class).
    {

        void Add(T item);// Method to add an item of type T to the repository.


        IReadOnlyList<T> GetAll();// Method to retrieve a read-only list of all items of type T in the repository.


        T? Find(Guid id);// Method to find an item of type T in the repository by its unique identifier (Guid). Returns null if not found.

    }
}
