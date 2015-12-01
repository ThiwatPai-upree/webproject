using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace DatabaseProject2015.Models
{
    public class Item
    {
        public Int64 ItemID { get; set; }
        public string ImageProfile { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string CompanyName { get; set; }
        public string Platform { get; set; }
        public decimal Cost { get; set; }
        public decimal SellingPrice { get; set; }
        public Int64 Quantity { get; set; }
        public string Rating { get; set; }
        public DateTime DateModified { get; set; }
    }
}