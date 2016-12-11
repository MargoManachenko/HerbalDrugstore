using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerbalDrugstore.Models
{
    public class Prompt
    {
        public Drug Drug { get; set; }

        public List<Supplier> Suppliers { get; set; }

        public string SupplierName { get; set; }

        public int ConsumePerDay { get; set; }

        public string Days { get; set; }

        public string Info { get; set; }
    }
}
