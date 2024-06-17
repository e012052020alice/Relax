using System.ComponentModel.DataAnnotations;

namespace RelaxService.Models
{
    public class AnalysisDetailN
    {
        [Key]
        public int AnalysisDId { get; set; }
        public int analysisId { get; set; }
        public string? UserIp { get; set; }
        public string? Device { get; set; }
        public int? SearchClicks { get; set; }
        public int? AdsClicks { get; set; }
        public int? LikeClicks { get; set; }
        public DateTime DataTime { get; set; }
    }
}
