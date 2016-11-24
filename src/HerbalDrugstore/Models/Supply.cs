using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HerbalDrugstore.Models
{
    public class Supply
    {
        [Key]
        [Required]
        public int SupplyId { get; set; }

        [Required]
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfSupply { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public float Price { get; set; }
    }
}
