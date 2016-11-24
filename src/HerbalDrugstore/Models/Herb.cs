using System.ComponentModel.DataAnnotations;

namespace HerbalDrugstore.Models
{
    public class Herb
    {
        [Required]
        [Key]
        public int HerbId { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Species { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
