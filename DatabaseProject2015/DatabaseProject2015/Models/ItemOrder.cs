using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject2015.Models
{
    public class ItemOrder
    {
        public Int64 ItemID { get; set; }
        public string Name { get; set; }
        public string ImageProfile { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
    }
}