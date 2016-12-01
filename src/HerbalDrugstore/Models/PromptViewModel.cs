using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerbalDrugstore.Models
{
    public class PromptViewModel
    {
        public Drug Drug { get; set; }

        public string SupplierName { get; set; }

        public int ConsumePerDay { get; set; }

        public string Days { get; set; }

        public string Info { get; set; }
    }
}
