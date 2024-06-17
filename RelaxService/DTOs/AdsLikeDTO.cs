using System.ComponentModel.DataAnnotations;

namespace RelaxService.DTOs
{
    public class AdsLikeDTO
    {
        [Key]
        public int AdsLikeId { get; set; }
        [MaxLength(100)]
        public string? memberId { get; set; }
        public int AdsId { get; set; }
        [MaxLength(50)]
        public string? Device { get; set; }
        [MaxLength(100)]
        public string? UserIp { get; set; }
        [MaxLength(50)]
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public DateTime AdsLikeTime { get; set; }
    }
}
