using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject2015.Models
{
    public class DeliveryMethod
    {
        public Int64 deliverymethodID { get; set; }
        public string deliverymethodname { get; set; }
        public int fee { get; set; }
    }
}