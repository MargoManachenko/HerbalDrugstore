using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HerbalDrugstore.Models
{
    public class Lot
    {
        [Required]
        [Key]
        public int LotId { get; set; }

        [Required]
        [ForeignKey("DrugId")]
        public virtual Drug Grug { get; set; }

        [Required]
        public int DrugId { get; set; }

        [Required]
        [Range(0, 50000)]
        public int Quantity { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public float Price { get; set; }

        [Required]
        [ForeignKey("SupplyId")]
        public virtual Supply Supply { get; set; }

        [Required]
        public int SupplyId { get; set; }
    }
}
