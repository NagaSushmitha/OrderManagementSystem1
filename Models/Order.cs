using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderManagementSystem.Models
{
    public class Order
    {
        public long OrderId { get; set; }
        public long BuyerId { get; set; }
        public short OrderStatusCode { get; set; }
        public string OrderStatusDesc { get; set; }
        public string ShippingAddress { get; set; }
        public long UserId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsDirty { get; set; }
        public bool IsDelete { get; set; }
        public List<Item> ItemsList { get; set; }
    }
}