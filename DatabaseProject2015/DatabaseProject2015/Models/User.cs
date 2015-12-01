using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject2015.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string SubDistrict { get; set; }
        public string Province { get; set; }
        public string Zipcode { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
       public string Birthday { get; set; }
    }
}