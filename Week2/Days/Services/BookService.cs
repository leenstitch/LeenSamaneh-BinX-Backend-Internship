/*
    File: BookService.cs

    Purpose:
    This file contains business logic related to books.

    Responsibility:
    - Communicates with IRepository<Book> instead of managing data directly.
    - Provides operations for adding and retrieving books.

    Used Files:
    - IBookService defines the service contract.
    - IRepository<Book> provides data access operations.
    - Book model represents the main entity.

    Concepts Applied:
    - Dependency Injection concept.
    - Service Layer Pattern.
    - Interface usage.

    Relation to Day 1:
    - Demonstrates using the generic repository with a specific model type (Book).
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Interfaces.RepositoryInterfaces;
using LibrarySystem.Models;

namespace LibrarySystem.Services
{
    public class BookService : IBookService
    {

        private readonly IRepository<Book> _repository;



        public BookService(
            IRepository<Book> repository)
        {
            _repository = repository;
        }



        public void AddBook(Book book)
        {
            _repository.Add(book);
        }



        public IReadOnlyList<Book> GetBooks()
        {
            return _repository.GetAll();
        }

    }
}
