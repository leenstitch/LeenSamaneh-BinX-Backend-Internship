// This file defines the contract for Author-related operations.
// Any class implementing this interface must provide these methods.
//this file is for week 3 - day 1
using APIProject.Dto_s.Week3Dto_s.AuthorBookDto_s;
using APIProject.Models;

namespace APIProject.Interfaces.InterfacesWeek3
{
    public interface IAuthorService
    {
        // This method retrieves all books written by a specific author based on the provided authorId.
        IEnumerable<AuthorBookResponseDto> GetBooksByAuthorId(int authorId);
    }
}
