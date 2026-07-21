using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Dtos;
using Library.Interfaces;

namespace Library.Payments
{
    // This class represents a card payment method that implements the IPayment interface.
    public class CardPayment : IPayment
    {
        // This method processes a payment using a card.
        public PaymentResult Pay(decimal amount)
        {

            return new PaymentResult(
                true,
                amount,
                "Card",
                "Payment completed successfully."
            );
        }
    }
}
