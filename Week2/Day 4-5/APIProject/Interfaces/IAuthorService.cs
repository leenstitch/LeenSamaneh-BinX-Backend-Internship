// This file defines the contract for Author-related operations.
// Any class implementing this interface must provide these methods.
using APIProject.Models;

namespace APIProject.Interfaces
{
    public interface IAuthorService
    {
        //======== Week 3 - Day 1 ========
        // This method retrieves all books written by a specific author based on the provided authorId.
        IEnumerable<Book> GetBooksByAuthorId(int authorId);
    }
}
