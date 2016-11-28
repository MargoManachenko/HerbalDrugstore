using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HerbalDrugstore.Models
{
    public class DrugAndHerbViewModel
    {
        public Drug Drug { get; set; }

        public List<Compound> Compound { get; set; }
    }
}
