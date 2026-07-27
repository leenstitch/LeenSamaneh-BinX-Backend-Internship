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
