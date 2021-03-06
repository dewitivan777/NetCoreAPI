﻿using System;

namespace OrderService.Models
{
    public class OrderEntity : BaseEntity
    {
        public string ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string State { get; set; }
    }
}
