using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HerbalDrugstore.Models
{
    public class Drug
    {
        [Required]
        [Key]
        public int DrugId { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        public string Indications { get; set; }

        [Required]
        public string Instruction { get; set; }

        [Required]
        [Range(0, 50000)]
        public int Quantity { get; set; }
    }
}
