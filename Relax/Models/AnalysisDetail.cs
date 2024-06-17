using System.ComponentModel.DataAnnotations;

namespace Relax.Models
{
    public class AnalysisDetail
    {
        [Key]
        public int AnalysisDId { get; set; }
        public int analysisId { get; set; }
        public string memberId { get; set; }
        public int SearchClicks { get; set; }
        public int AdsClicks { get; set; }
        public int LikeClicks { get; set; }

        public DateTime DataTime { get; set; }
    }
}
