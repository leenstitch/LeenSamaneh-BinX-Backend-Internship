/*
    File: OrderDto.cs

    Purpose:
    This file defines a Data Transfer Object for transferring order information.

    Responsibility:
    - Stores simplified order information.
    - Prevents exposing the complete Order model directly.

    Used Files:
    - Order model:
      The DTO contains selected order data such as Id and Total.

    Project Layer:
    DTOs are used as a communication model between application layers.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DTOs.OrderDto_s
{
    // This record represents a Data Transfer Object (DTO) for an order.
    public record OrderDto(
       Guid Id,
       string CustomerName,
       decimal Total
   );
}
