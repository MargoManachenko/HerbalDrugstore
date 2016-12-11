using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HerbalDrugstore.Models
{
    public class DrugChanges
    {
        [Required]
        [Key]
        public int ChangeId { get; set; }

        public int DrugId { get; set; }

        [ForeignKey("DrugId")]
        public virtual Drug Drug { get; set; }

        public bool Increasing { get; set; }

        public int Quantity { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string SupplierName { get; set; }
    }
}
