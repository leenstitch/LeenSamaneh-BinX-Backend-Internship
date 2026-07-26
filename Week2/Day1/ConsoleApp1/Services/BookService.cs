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
