using System.Collections.Generic;
using System.Web.Mvc;

namespace NacostiEProcMVC.Models
{
    public class DropdownListsModel
    {
        public List<SelectListItem> MyDropdownList { get; set; }
        public int? PostalId { get; set; }
        public string PostaCode { get; set; }

        public int? CountryId { get; set; }
        public string CountryName { get; set; }

        public int? SupplierCategoryId { get; set; }
        public string SupplierCategory { get; set; }

    }
}