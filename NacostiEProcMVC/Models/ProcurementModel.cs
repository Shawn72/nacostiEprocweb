using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NacostiEProcMVC.Models
{
    public class ProcurementModel
    {
        public  int EntryNo { get; set; }
        public string No { get; set; }
        public string Title { get; set; }
        public string SupplierCategory { get; set; }
        public DateTime Return_Date { get; set; }
        public string Return_Time { get; set; }
        public string Status { get; set; }
        public string E_Mail { get; set; }
        public string Process_Type { get; set; }
        public bool Quotation_Finished { get; set; }


    }
}