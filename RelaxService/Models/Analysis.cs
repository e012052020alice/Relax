using System.ComponentModel.DataAnnotations;

namespace RelaxService.Models
{
    public class Analysis
    {
        [Key]
        public int AnalysisId { get; set; }
        public int? TotalUser { get; set; }
        public int? TotalEnter { get; set; }
        public int? EnterDesktop { get; set; }
        public int? EnterMobile { get; set; }
        public int? TotalSearch { get; set; }
        public int? SearchDesktop { get; set; }
        public int? SearchMobile { get; set; }
        public int? TotalAds { get; set; }
        public int? AdsDesktop { get; set; }
        public int? AdsMobile { get; set; }
        public int? TotalLike { get; set; }
        public int? LikeDesktop { get; set; }
        public int? LikeMobile { get; set; }
        public DateTime DataDate { get; set; }
    }
}
