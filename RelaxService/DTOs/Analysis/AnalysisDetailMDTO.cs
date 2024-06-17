using System.ComponentModel.DataAnnotations;

namespace RelaxService.DTOs.Analysis
{
    public class AnalysisDetailMDTO
    {
        [Key]
        public int AnalysisDId { get; set; }
        public int analysisId { get; set; }
        [MaxLength(100)]
        public string? memberId { get; set; }
        [MaxLength(50)]
        public string? Device { get; set; }
        public int? SearchClicks { get; set; }
        public int? AdsClicks { get; set; }
        public int? LikeClicks { get; set; }
        public DateTime DataTime { get; set; }
    }
}
