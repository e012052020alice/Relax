using System.ComponentModel.DataAnnotations;

namespace RelaxService.DTOs.Analysis
{
    public class GenderSumDTO
    {
        public int? SearchM {  get; set; }
        public int? SearchF { get; set; }
        public int? AdsM { get; set; }
        public int? AdsF { get; set; }
        public int? LikeM { get; set; }
        public int? LikeF { get; set; }

    }
}
