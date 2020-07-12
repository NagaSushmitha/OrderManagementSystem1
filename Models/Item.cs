using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderManagementSystem.Models
{
    public class Item
    {
        public long ItemId { get; set; }
        public long OrderId { get; set; }
        public int Quantity { get; set; }
        public bool IsDirty { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int StatusCode { get; set; } 
        public bool IsDelete { get; set; }      
        public string Name { get; set; }
        public decimal Weight { get; set; }
        public string ImageUrl { get; set; }
        public int Height { get; set; }
        public long SKUId { get; set; }
        public int AvailableQuantity { get; set; }
    }
}