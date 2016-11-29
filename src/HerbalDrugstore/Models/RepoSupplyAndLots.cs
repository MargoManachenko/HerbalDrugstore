using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerbalDrugstore.Models
{
    public class RepoSupplyAndLots
    {
        public List<Lot> Lots { get; set; }

        public List<Supply> Supply { get; set; }
    }
}
