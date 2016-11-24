using System.ComponentModel.DataAnnotations;

namespace HerbalDrugstore.Models
{
    public class Supplier
    {
        [Required]
        [Key]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(25)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(30)]
        public string ContactName { get; set; }

        [Required]
        [StringLength(20)]
        public string Country { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}
