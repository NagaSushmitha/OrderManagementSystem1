using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderManagementSystem.Models
{
    public class Product
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public decimal Weight { get; set; }
        public int Height { get; set; }
        public long SKUId { get; set; }
        public int AvailableQuantity { get; set; }

    }
}