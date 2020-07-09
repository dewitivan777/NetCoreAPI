using System.Collections.Generic;

namespace ClassificationService.Models
{
    public class CategoryEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }
    }
}
