using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerbalDrugstore.Models
{
    public class MakeBigOrderViewModel
    {
        public List<DrugStatistics> DrugStatisticses { get; set; }

        public string SupplierName { get; set; }

        public List<string> SuppliresCompanies { get; set; }
    }
}
