namespace ProductService.Models
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; }
        public string SupplierId { get; set; }
        public string CategoryId { get; set; }
        public int QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
    }
}
