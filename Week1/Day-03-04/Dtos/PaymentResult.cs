using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dtos
{
    // This record represents the result of a payment operation.
    public record PaymentResult(
        bool Success,
        decimal Amount,
        string PaymentMethod,
        string Message
    );
}
