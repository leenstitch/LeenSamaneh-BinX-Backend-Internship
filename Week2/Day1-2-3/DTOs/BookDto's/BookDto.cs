/*
    File: BookDto.cs

    Purpose:
    This file defines a Data Transfer Object (DTO) for transferring book data.

    Responsibility:
    - Provides a simplified representation of a Book object.
    - Exposes only the required data that should be transferred between layers.

    Used Files:
    - Book model:
      The DTO represents selected information from the Book entity.

    Project Layer:
    DTOs layer is used to separate the internal models from the data exposed
    to other parts of the application.
*/


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
