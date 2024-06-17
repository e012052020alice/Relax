using System.ComponentModel.DataAnnotations;

namespace RelaxService.DTOs.Analysis
{
    public class AdsDetailNDTO
    {
        [Key]
        public int AdsDId { get; set; }
        public int adsId { get; set; }
        public int AnalysisId { get; set; }
        [MaxLength(50)]
        public string? Device { get; set; }
        [MaxLength(100)]
        public string? UserIp { get; set; }
        public int? AdsClick { get; set; }
        public int? LikeClick { get; set; }
        public DateTime DataDate { get; set; }
    }
}
