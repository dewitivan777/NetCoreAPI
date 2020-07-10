using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Models
{
    public class Sort
    {
        public Sort(string field, SortDirection direction)
        {
            Field = field;
            Direction = direction;
        }

        public string Field { get; set; }
        public SortDirection Direction { get; set; }

        public enum SortDirection
        {
            Ascending,
            Descending
        }
    }
}
