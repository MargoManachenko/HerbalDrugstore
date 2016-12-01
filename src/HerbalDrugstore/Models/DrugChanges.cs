using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HerbalDrugstore.Models
{
    public class DrugChanges
    {
        [Required]
        [Key]
        public int ChangeId { get; set; }

        [Required]
        public int DrugId { get; set; }

        [Required]
        public virtual Drug Drug { get; set; }

        [Required]
        public bool Increasing { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string SupplierName { get; set; }
    }
}
