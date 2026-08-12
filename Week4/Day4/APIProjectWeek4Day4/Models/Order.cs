// a Order model class to represent the Order entity

namespace APIProject.Models
{
    public class Order
    {
        public int Id { get; set; }

        public decimal Total { get; set; } = 0;

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
