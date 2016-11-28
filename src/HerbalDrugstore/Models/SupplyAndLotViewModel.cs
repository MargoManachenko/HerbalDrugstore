using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerbalDrugstore.Models
{
    public class SupplyAndLotViewModel
    {
        public Supply Supply { get; set; }

        public List<Lot> Lots { get; set; }
    }
}
