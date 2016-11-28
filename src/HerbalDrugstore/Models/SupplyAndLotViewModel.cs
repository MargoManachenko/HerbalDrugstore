using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerbalDrugstore.Models
{
    public class SupplyAndLotViewModel
    {
        public Supplier Supplier { get; set; }

        public Supply Supply { get; set; }

        public List<Lot> Lots { get; set; }

        public List<Drug> Drugs { get; set; }
    }
}
