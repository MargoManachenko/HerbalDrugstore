using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HerbalDrugstore.Models
{
    public class Compound
    {
        [Required]
        public int Quantity { get; set; }

        [Key, Column(Order = 0)]
        [Required]
        public int DrugId { get; set; }

        [Key, Column(Order = 1)]
        [Required] 
        public int HerbId { get; set; }

        [Required]
        public virtual Drug Drug { get; set; }

        [Required]
        public virtual Herb Herb { get; set; }
    }
}
