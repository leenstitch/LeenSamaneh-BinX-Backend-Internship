/*
    File: IRepository.cs

    Purpose:
    This file defines a generic repository contract.

    Responsibility:
    - Provides common operations for managing different entity types.
    - Uses generics to allow the same repository logic to work with
      Books, Customers, Orders, and other models.

    Used Files:
    - Repository<T>:
      Implements this interface.

     Day 1 Concepts Applied:
    - Generic interfaces using type parameter T.
    - Generic constraint where T : class.
    - Repository Pattern.
    - IReadOnlyList<T> to prevent direct modification of returned data.
*/

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


        T? Find(Func<T, bool> predicate);// Method to find an item of type T in the repository by its unique identifier (Guid). Returns null if not found.

    }
}
