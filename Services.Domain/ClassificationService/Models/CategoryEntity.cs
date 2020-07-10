using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Domain.ClassificationService.Models
{
    public class CategoryEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
