using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NacostiEProcMVC.Models
{
    public class TenderModel
    {
        public int EntryNo { get; set; }
        public string Ref_No { get; set; }
        public string Name { get; set; }
        public string Vendor_No { get; set; }
        public string Title { get; set; }
        public bool Prequalified { get; set; }
        public bool Selected { get; set; }
        public  bool Successful { get; set; }
        public string Contact_No { get; set; }
        public string E_Mail { get; set; }
    }
}