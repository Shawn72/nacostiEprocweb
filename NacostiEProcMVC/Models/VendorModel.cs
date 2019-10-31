using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NacostiEProcMVC.Models
{
    public class VendorModel
    {
        public List<SelectListItem> VendorItems { get; set; }
        public string VendorName { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Phonenumber { get; set; }
        public string Taxpin { get; set; }
        public string KraPin { get; set; }
        public string Email { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }
    }
}