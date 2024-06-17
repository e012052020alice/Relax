using System.ComponentModel.DataAnnotations;

namespace RelaxService.DTOs
{
    public class AdsDTO
    {
        [Key]
        public int AdId { get; set; }
        [MaxLength(100)]
        public string? memberId { get; set; }
        public int? tripId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        //varbinary(MAX):是否需要調整
        public string? Image { get; set; }
        [MaxLength(200)]
        public string? Detail { get; set; }
        //廣告點擊次數
        public int? Clicks { get; set; }
        //廣告按愛心次數
        public int? AdLoved { get; set; }
        //廣告追蹤url
        public string? AdUrl { get; set; }
    }
}
