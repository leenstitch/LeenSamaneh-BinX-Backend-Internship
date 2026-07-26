using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DTOs.BookDto_s
{
    // This record represents a Data Transfer Object (DTO) for a book, containing its unique identifier, title, and price.
    public record BookDto(
     Guid Id,
     string Title,
     decimal Price
 );
}
