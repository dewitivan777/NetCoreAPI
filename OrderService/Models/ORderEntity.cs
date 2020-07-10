
namespace OrderService.Models
{
    public class OrderEntity : BaseEntity
    {
        public string Name { get; set; }
        public string ProductId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public State state { get; set; }
    }

    public enum State
    {
        Canceled,
        Processing,
        Complete
    }


}
