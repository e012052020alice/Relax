using System.ComponentModel.DataAnnotations;

namespace RelaxService.Models
{
    public class GenderAds
    {
        [Key]
        public int GenderAId { get; set; }
        [MaxLength(50)]
        public string? Device { get; set; }
        [MaxLength(50)]
        public string? Gender { get; set; }
        public int? Age18 { get; set;}
        public int? Age25 { get; set; }
        public int? Age35 { get; set; }
        public int? Age45 { get; set; }
        public int? Age65 { get; set; }
        public DateTime DataDate { get; set; }
    }
}
