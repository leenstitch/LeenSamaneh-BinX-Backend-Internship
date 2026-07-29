/*
    File: Repository.cs

    Purpose:
    This file contains the generic repository implementation.

    Responsibility:
    - Stores and manages entities using a generic List<T>.
    - Implements Add, GetAll, and Find operations.
    - Provides reusable repository logic for different models.

    Used Files:
    - IRepository<T> defines the required operations.
    - Book and Customer models are examples of entities stored in this repository.

    Day 1 Concepts Applied:
    - Generic classes using Repository<T>.
    - Generic constraint where T : class.
    - Generic methods working with different entity types.
    - IReadOnlyList<T> to return read-only collections.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Interfaces.RepositoryInterfaces;

namespace LibrarySystem.Repositories
{
    // Generic repository class that implements IRepository<T> interface
    public class Repository<T> : IRepository<T>
    where T : class // Generic type constraint to ensure T is a reference type
    {

        protected readonly List<T> Items = new();// Internal list to store items of type T


        // Method to add an item of type T to the repository
        public void Add(T item)
        {
            Items.Add(item);
        }


        // Method to remove an item of type T from the repository
        public IReadOnlyList<T> GetAll()
        {
            return Items.AsReadOnly();
        }


        // Method to find an item of type T by its unique identifier (Guid)
        public T? Find(Func<T, bool> predicate)
        {
            return Items.FirstOrDefault(predicate);
        }

    }
}
