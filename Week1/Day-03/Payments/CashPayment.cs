using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Dtos;
using Library.Interfaces;

namespace Library.Payments
{
    // This class represents a cash payment method that implements the IPayment interface.
    public class CashPayment : IPayment
    {
        // This method processes a payment using cash.
        public PaymentResult Pay(decimal amount)
        {
            return new PaymentResult(
                true,
                amount,
                "Cash",
                "Payment completed successfully."
            );
        }
    }
}
