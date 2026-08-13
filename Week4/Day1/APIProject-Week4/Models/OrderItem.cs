// a OrderItem model class to represent the OrderItem entity
namespace APIProject.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int Quantity { get; set; } = 0;

        public decimal Subtotal { get; set; } = 0;

        public int OrderId { get; set; }

        public Order Order { get; set; } = null!;

        public int BookId { get; set; }

        public Book Book { get; set; } = null!;
    }
}
