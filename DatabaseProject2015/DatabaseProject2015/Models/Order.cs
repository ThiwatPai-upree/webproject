using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject2015.Models
{
    public class Order
    {
        public Int64 OrderID { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal Fee { get; set; }
        public string OrderStatus { get; set; }
        public DateTime DateAdded { get; set; }
        public decimal Total { get; set; }
    }
}