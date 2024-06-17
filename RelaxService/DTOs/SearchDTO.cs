using System.ComponentModel.DataAnnotations;

namespace RelaxService.DTOs
{
    public class SearchDTO
    {
        [Key]
        public int SearchId { get; set; }
        [MaxLength(100)]
        public string? memberId { get; set; }
        [MaxLength(100)]
        public string? SearchIp { get; set; }
        [MaxLength(50)]
        public string? Device { get; set; }
        [MaxLength(200)]
        public string? Keywords { get; set; }
        [MaxLength(50)]
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public DateTime SearchTime { get; set; }
    }
}
