using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Dtos;

namespace Library.Interfaces
{
    // This interface represents a payment method.
    public interface IPayment
    {
        PaymentResult Pay(decimal amount);
    }
}
