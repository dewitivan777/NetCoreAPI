using System.Collections.Generic;

namespace ClassificationService.Models
{
    public class ProductEntity 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SupplierId { get; set; }
        public string CategoryId { get; set; }
        public int QuantityPerUnit { get; set; }
        public double UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
    }
}
