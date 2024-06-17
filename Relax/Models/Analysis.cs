using System.ComponentModel.DataAnnotations;

namespace Relax.Models
{
    public class Analysis
    {
        [Key]
        public int AnalysisId { get; set; }
        public int TotalEnter { get; set; }
        public int TotalVisitors { get; set; }
        public int TotalSearch { get; set; }
        public int TotalAds { get; set; }
        public int TotalLike { get; set; }
        public DateTime DataDate { get; set; }
    }
}
