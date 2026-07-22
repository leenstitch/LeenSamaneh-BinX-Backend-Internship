using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dtos
{
    // This record represents an order with its details.
    public record OrderDto(
    Guid Id,
    string CustomerName,
    decimal TotalPrice
);
}
