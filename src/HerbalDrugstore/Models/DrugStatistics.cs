using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerbalDrugstore.Models
{
    public class DrugStatistics
    {
        public string DrugName { get; set; }

        public double DrugPrice { get; set; }

        public int UnitsPerDay { get; set; }

        public int Recomended { get; set; }

        public string SupplierName { get; set; }
    }
}
